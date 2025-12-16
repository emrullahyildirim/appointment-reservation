using Core.CrossCuttingConcerns.Caching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Core.Utilities.Security.S2S
{
    /// <summary>
    /// S2S Token Client implementasyonu
    /// AuthService'den token alır ve distributed cache'e kaydeder (Redis/Memory)
    /// Kubernetes multi-pod ortamında güvenli çalışır
    /// </summary>
    public class S2STokenClient : IS2STokenClient
    {
        private readonly HttpClient _httpClient;
        private readonly S2SClientOptions _options;
        private readonly ILogger<S2STokenClient> _logger;
        private readonly ICacheManager _cacheManager;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        
        private const string CacheKeyPrefix = "s2s_token_";

        public S2STokenClient(
            HttpClient httpClient,
            IOptions<S2SClientOptions> options,
            ILogger<S2STokenClient> logger,
            ICacheManager cacheManager)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
            _cacheManager = cacheManager;
            
            _httpClient.BaseAddress = new Uri(_options.AuthServiceUrl.TrimEnd('/') + "/");
        }

        public async Task<string> GetTokenAsync(string? scope = null, CancellationToken cancellationToken = default)
        {
            var requestedScope = scope ?? _options.DefaultScopes ?? "";
            var cacheKey = GetCacheKey(requestedScope);
            
            // Distributed cache'den token'ı kontrol et
            if (_cacheManager.IsAdd(cacheKey))
            {
                try
                {
                    var cachedData = _cacheManager.Get<S2STokenCacheData>(cacheKey);
                    if (cachedData != null && 
                        cachedData.ExpiresAt > DateTime.UtcNow.AddMinutes(_options.TokenRefreshBufferMinutes))
                    {
                        _logger.LogDebug("S2S token retrieved from cache for scope: {Scope}", requestedScope);
                        return cachedData.AccessToken;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to get S2S token from cache, will request new token");
                }
            }

            // Token al (thread-safe - aynı pod içinde race condition önler)
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                // Double-check pattern 
                if (_cacheManager.IsAdd(cacheKey))
                {
                    try
                    {
                        var cachedData = _cacheManager.Get<S2STokenCacheData>(cacheKey);
                        if (cachedData != null && 
                            cachedData.ExpiresAt > DateTime.UtcNow.AddMinutes(_options.TokenRefreshBufferMinutes))
                        {
                            return cachedData.AccessToken;
                        }
                    }
                    catch { /* Cache'den okunamadıysa yeni token al */ }
                }

                // AuthService'den yeni token al
                var tokenResponse = await RequestTokenAsync(requestedScope, cancellationToken);
                
                // Token'ı distributed cache'e kaydet
                var cacheData = new S2STokenCacheData
                {
                    AccessToken = tokenResponse.AccessToken,
                    ExpiresAt = tokenResponse.ExpiresAt,
                    Scope = requestedScope
                };
                
                // Cache süresi: token expire süresinden buffer kadar önce
                var cacheDurationMinutes = (int)(tokenResponse.ExpiresAt - DateTime.UtcNow).TotalMinutes 
                    - _options.TokenRefreshBufferMinutes;
                
                if (cacheDurationMinutes > 0)
                {
                    _cacheManager.Add(cacheKey, cacheData, cacheDurationMinutes);
                }
                
                _logger.LogDebug("S2S token acquired and cached for scope: {Scope}, expires at: {Expiry}", 
                    requestedScope, tokenResponse.ExpiresAt);
                
                return tokenResponse.AccessToken;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void InvalidateToken(string? scope = null)
        {
            var requestedScope = scope ?? _options.DefaultScopes ?? "";
            var cacheKey = GetCacheKey(requestedScope);
            
            if (_cacheManager.IsAdd(cacheKey))
            {
                _cacheManager.Remove(cacheKey);
                _logger.LogDebug("S2S token cache invalidated for scope: {Scope}", requestedScope);
            }
        }

        public void InvalidateAllTokens()
        {
            // Tüm S2S token'larını temizle
            _cacheManager.RemoveByPattern($"{CacheKeyPrefix}*");
            _logger.LogDebug("All S2S token caches invalidated for client: {ClientId}", _options.ClientId);
        }

        private string GetCacheKey(string scope)
        {
            // Her client ve scope için benzersiz cache key
            // Format: s2s_token_{clientId}_{scope_hash}
            var scopeHash = string.IsNullOrEmpty(scope)
                ? "default"
                : StableScopeHash(scope);
            return $"{CacheKeyPrefix}{_options.ClientId}_{scopeHash}";
        }


        private static string StableScopeHash(string scope)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(scope));
            return Convert.ToHexString(bytes)[..8];
        }

        private async Task<S2STokenResponseDto> RequestTokenAsync(string? scope, CancellationToken cancellationToken)
        {
            var request = new S2STokenRequestDto
            {
                ClientId = _options.ClientId,
                ClientSecret = _options.ClientSecret,
                Scope = scope
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/s2s/token", request, cancellationToken);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError("Failed to get S2S token. Status: {Status}, Error: {Error}", 
                        response.StatusCode, errorContent);
                    throw new S2STokenException($"Failed to get S2S token: {response.StatusCode}");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponseWrapper<S2STokenResponseDto>>(
                    cancellationToken: cancellationToken);
                
                if (result?.Data == null)
                {
                    throw new S2STokenException("Invalid token response from AuthService");
                }

                return result.Data;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error while getting S2S token from AuthService");
                throw new S2STokenException("Failed to connect to AuthService", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse S2S token response");
                throw new S2STokenException("Invalid response format from AuthService", ex);
            }
        }

        /// <summary>
        /// API response wrapper for deserialization
        /// </summary>
        private class ApiResponseWrapper<T>
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public T? Data { get; set; }
        }
    }

    /// <summary>
    /// Cache'e kaydedilecek token verisi
    /// </summary>
    [Serializable]
    public class S2STokenCacheData
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Scope { get; set; } = string.Empty;
    }

    /// <summary>
    /// S2S Token alma hatası
    /// </summary>
    public class S2STokenException : Exception
    {
        public S2STokenException(string message) : base(message) { }
        public S2STokenException(string message, Exception inner) : base(message, inner) { }
    }
}
