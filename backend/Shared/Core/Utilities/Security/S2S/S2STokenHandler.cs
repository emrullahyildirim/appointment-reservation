using System.Net.Http.Headers;

namespace Core.Utilities.Security.S2S
{
    /// <summary>
    /// HttpClient DelegatingHandler
    /// Otomatik olarak S2S token'ı Authorization header'ına ekler
    /// </summary>
    public class S2STokenHandler : DelegatingHandler
    {
        private readonly IS2STokenClient _tokenClient;
        private readonly string? _scope;

        public S2STokenHandler(IS2STokenClient tokenClient, string? scope = null)
        {
            _tokenClient = tokenClient;
            _scope = scope;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            // Token al ve header'a ekle
            var token = await _tokenClient.GetTokenAsync(_scope, cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await base.SendAsync(request, cancellationToken);
            
            // 401 Unauthorized dönerse token'ı invalidate et ve tekrar dene
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _tokenClient.InvalidateToken(_scope);
                
                // Yeni token al
                token = await _tokenClient.GetTokenAsync(_scope, cancellationToken);
                
                // Yeni request oluştur (aynı request tekrar gönderilemez)
                var retryRequest = await CloneHttpRequestMessageAsync(request);
                retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                // Tekrar dene
                response = await base.SendAsync(retryRequest, cancellationToken);
            }
            
            return response;
        }

        /// <summary>
        /// HttpRequestMessage'ı klonlar (retry için gerekli)
        /// </summary>
        private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version
            };

            // Content'i kopyala
            if (request.Content != null)
            {
                var contentBytes = await request.Content.ReadAsByteArrayAsync();
                clone.Content = new ByteArrayContent(contentBytes);

                // Content headers'ı kopyala
                foreach (var header in request.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            // Headers'ı kopyala (Authorization hariç - onu yeni ekleyeceğiz)
            foreach (var header in request.Headers)
            {
                if (header.Key != "Authorization")
                {
                    clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            // Options'ları kopyala
            foreach (var option in request.Options)
            {
                clone.Options.TryAdd(option.Key, option.Value);
            }

            return clone;
        }
    }
}
