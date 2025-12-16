using AuthService.Business.Abstract;
using AuthService.Business.Constants;
using AuthService.DataAccess.Abstract;
using AuthService.Entities.Concrete;
using AuthService.Entities.DTOs;
using Core.Extensions;
using Core.Utilities.Result;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Core.Utilities.Security.S2S;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthService.Business.Concrete
{
    /// <summary>
    /// Service-to-Service token yönetimi
    /// OAuth 2.0 Client Credentials Grant implementasyonu
    /// </summary>
    public class S2STokenManager : IS2STokenService
    {
        private readonly IServiceClientDal _serviceClientDal;
        private readonly TokenOptions _tokenOptions;

        public S2STokenManager(
            IServiceClientDal serviceClientDal,
            IOptions<TokenOptions> tokenOptions)
        {
            _serviceClientDal = serviceClientDal;
            _tokenOptions = tokenOptions.Value;
        }

        /// <summary>
        /// Client Credentials ile S2S token oluşturur
        /// </summary>
        public IDataResult<S2STokenResponseDto> CreateToken(S2STokenRequestDto request)
        {
            // ClientId ile servisi bul
            var serviceClient = _serviceClientDal.GetByClientId(request.ClientId);
            
            if (serviceClient == null)
                return new ErrorDataResult<S2STokenResponseDto>(Messages.InvalidClientCredentials);

            if (!serviceClient.IsActive)
                return new ErrorDataResult<S2STokenResponseDto>(Messages.ServiceClientDeactivated);

            // ClientSecret doğrula
            if (!HashingHelper.VerifyPasswordHash(request.ClientSecret, 
                serviceClient.ClientSecretHash, serviceClient.ClientSecretSalt))
            {
                return new ErrorDataResult<S2STokenResponseDto>(Messages.InvalidClientCredentials);
            }

            // Scope kontrolü
            var requestedScopes = ParseScopes(request.Scope);
            var allowedScopes = ParseScopes(serviceClient.AllowedScopes);
            
            // İstenen scope'ların izinli olup olmadığını kontrol et
            var grantedScopes = requestedScopes.Count > 0
                ? requestedScopes.Where(s => allowedScopes.Contains(s)).ToList()
                : allowedScopes;

            if (requestedScopes.Count > 0 && grantedScopes.Count == 0)
                return new ErrorDataResult<S2STokenResponseDto>(Messages.InvalidScope);

            // S2S Token oluştur
            var token = CreateS2SAccessToken(serviceClient, grantedScopes);
            
            return new SuccessDataResult<S2STokenResponseDto>(token, Messages.S2STokenCreated);
        }

        /// <summary>
        /// Token doğrulama (hem user hem service token)
        /// </summary>
        public IDataResult<TokenValidationResponseDto> ValidateToken(TokenValidationRequestDto request)
        {
            var response = new TokenValidationResponseDto();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _tokenOptions.Issuer,
                    ValidAudience = _tokenOptions.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(request.Token, validationParameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                // Token tipini belirle (service vs user)
                var tokenTypeClaim = principal.FindFirst("token_type")?.Value;
                var isServiceToken = tokenTypeClaim == "service";

                response.IsValid = true;
                response.TokenType = isServiceToken ? "service" : "user";
                response.ExpiresAt = jwtToken.ValidTo;

                if (isServiceToken)
                {
                    response.SubjectId = principal.FindFirst("client_id")?.Value;
                    response.SubjectName = principal.FindFirst("service_name")?.Value;
                    
                    // Scope'ları al
                    var scopeClaim = principal.FindFirst("scope")?.Value;
                    response.Claims = ParseScopes(scopeClaim);
                }
                else
                {
                    response.SubjectId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    response.SubjectName = principal.FindFirst(ClaimTypes.Email)?.Value;
                    
                    // Rolleri al
                    response.Claims = principal.FindAll(ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList();
                }

                // Required scope kontrolü
                if (!string.IsNullOrEmpty(request.RequiredScope))
                {
                    if (!response.Claims.Contains(request.RequiredScope))
                    {
                        response.IsValid = false;
                        response.ErrorMessage = Messages.InsufficientScope;
                    }
                }

                return new SuccessDataResult<TokenValidationResponseDto>(response);
            }
            catch (SecurityTokenExpiredException)
            {
                response.IsValid = false;
                response.ErrorMessage = Messages.TokenExpired;
                return new ErrorDataResult<TokenValidationResponseDto>(response, Messages.TokenExpired);
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                response.ErrorMessage = Messages.InvalidToken;
                return new ErrorDataResult<TokenValidationResponseDto>(response, Messages.InvalidToken);
            }
        }

        /// <summary>
        /// Yeni ServiceClient oluşturur
        /// </summary>
        public IDataResult<ServiceClientCreatedDto> CreateServiceClient(ServiceClientDto dto)
        {
            // Aynı isimde servis var mı kontrol et
            var existingService = _serviceClientDal.GetByServiceName(dto.ServiceName);
            if (existingService != null)
                return new ErrorDataResult<ServiceClientCreatedDto>(Messages.ServiceClientAlreadyExists);

            // ClientId ve ClientSecret oluştur
            var clientId = GenerateClientId();
            var clientSecret = GenerateClientSecret();

            // Secret'ı hashle
            HashingHelper.CreatePasswordHash(clientSecret, out byte[] secretHash, out byte[] secretSalt);

            var serviceClient = new ServiceClient
            {
                ServiceName = dto.ServiceName,
                ClientId = clientId,
                ClientSecretHash = secretHash,
                ClientSecretSalt = secretSalt,
                AllowedScopes = dto.AllowedScopes,
                TokenExpirationMinutes = dto.TokenExpirationMinutes,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _serviceClientDal.Add(serviceClient);

            var result = new ServiceClientCreatedDto
            {
                Id = serviceClient.Id,
                ServiceName = serviceClient.ServiceName,
                ClientId = clientId,
                ClientSecret = clientSecret, // Sadece bir kez gösterilir!
                AllowedScopes = serviceClient.AllowedScopes,
                TokenExpirationMinutes = serviceClient.TokenExpirationMinutes
            };

            return new SuccessDataResult<ServiceClientCreatedDto>(result, Messages.ServiceClientCreated);
        }

        /// <summary>
        /// ServiceClient günceller
        /// </summary>
        public IResult UpdateServiceClient(string clientId, ServiceClientDto dto)
        {
            var serviceClient = _serviceClientDal.GetByClientId(clientId);
            if (serviceClient == null)
                return new ErrorResult(Messages.ServiceClientNotFound);

            serviceClient.AllowedScopes = dto.AllowedScopes;
            serviceClient.TokenExpirationMinutes = dto.TokenExpirationMinutes;
            serviceClient.UpdatedDate = DateTime.UtcNow;

            _serviceClientDal.Update(serviceClient);

            return new SuccessResult(Messages.ServiceClientUpdated);
        }

        /// <summary>
        /// ServiceClient deaktif eder
        /// </summary>
        public IResult DeactivateServiceClient(string clientId)
        {
            var serviceClient = _serviceClientDal.GetByClientId(clientId);
            if (serviceClient == null)
                return new ErrorResult(Messages.ServiceClientNotFound);

            serviceClient.IsActive = false;
            serviceClient.UpdatedDate = DateTime.UtcNow;

            _serviceClientDal.Update(serviceClient);

            return new SuccessResult(Messages.ServiceClientDeactivated);
        }

        /// <summary>
        /// ClientSecret yeniler
        /// </summary>
        public IDataResult<ServiceClientCreatedDto> RegenerateClientSecret(string clientId)
        {
            var serviceClient = _serviceClientDal.GetByClientId(clientId);
            if (serviceClient == null)
                return new ErrorDataResult<ServiceClientCreatedDto>(Messages.ServiceClientNotFound);

            var newSecret = GenerateClientSecret();
            HashingHelper.CreatePasswordHash(newSecret, out byte[] secretHash, out byte[] secretSalt);

            serviceClient.ClientSecretHash = secretHash;
            serviceClient.ClientSecretSalt = secretSalt;
            serviceClient.UpdatedDate = DateTime.UtcNow;

            _serviceClientDal.Update(serviceClient);

            var result = new ServiceClientCreatedDto
            {
                Id = serviceClient.Id,
                ServiceName = serviceClient.ServiceName,
                ClientId = serviceClient.ClientId,
                ClientSecret = newSecret,
                AllowedScopes = serviceClient.AllowedScopes,
                TokenExpirationMinutes = serviceClient.TokenExpirationMinutes
            };

            return new SuccessDataResult<ServiceClientCreatedDto>(result, Messages.ClientSecretRegenerated);
        }

        #region Private Methods

        private S2STokenResponseDto CreateS2SAccessToken(ServiceClient serviceClient, List<string> scopes)
        {
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            
            var expirationMinutes = serviceClient.TokenExpirationMinutes > 0 
                ? serviceClient.TokenExpirationMinutes 
                : 60;
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var claims = new List<Claim>
            {
                new Claim("token_type", "service"),
                new Claim("client_id", serviceClient.ClientId),
                new Claim("service_name", serviceClient.ServiceName),
                new Claim("scope", string.Join(" ", scopes)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Her scope'u ayrı claim olarak da ekle (kolay kontrol için)
            foreach (var scope in scopes)
            {
                claims.Add(new Claim(ClaimTypes.Role, scope));
            }

            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: expiration,
                notBefore: DateTime.UtcNow,
                claims: claims,
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(jwt);

            return new S2STokenResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = expirationMinutes * 60, // saniye cinsinden
                ExpiresAt = expiration,
                Scope = string.Join(" ", scopes)
            };
        }

        private static string GenerateClientId()
        {
            // Format: svc_xxxxxxxxxxxx (16 karakter random)
            return $"svc_{GenerateRandomString(16)}";
        }

        private static string GenerateClientSecret()
        {
            // 32 byte random = 256 bit güvenlik
            return GenerateRandomString(48);
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var randomBytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[randomBytes[i] % chars.Length];
            }

            return new string(result);
        }

        private static List<string> ParseScopes(string? scopeString)
        {
            if (string.IsNullOrWhiteSpace(scopeString))
                return new List<string>();

            return scopeString
                .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .Distinct()
                .ToList();
        }

        #endregion
    }
}
