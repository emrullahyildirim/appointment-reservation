using AuthService.Entities.DTOs;
using Core.Utilities.Result;
using Core.Utilities.Security.S2S;

namespace AuthService.Business.Abstract
{
    /// <summary>
    /// Service-to-Service (S2S) token işlemleri için interface
    /// OAuth 2.0 Client Credentials Grant implementasyonu
    /// </summary>
    public interface IS2STokenService
    {
        /// <summary>
        /// Client Credentials ile S2S token oluşturur
        /// </summary>
        /// <param name="request">ClientId ve ClientSecret içeren request</param>
        /// <returns>JWT Access Token</returns>
        IDataResult<S2STokenResponseDto> CreateToken(S2STokenRequestDto request);
        
        /// <summary>
        /// Token'ı doğrular (hem user hem de service token'ları için)
        /// </summary>
        /// <param name="request">Doğrulanacak token</param>
        /// <returns>Token bilgileri</returns>
        IDataResult<TokenValidationResponseDto> ValidateToken(TokenValidationRequestDto request);
        
        /// <summary>
        /// Yeni bir ServiceClient oluşturur
        /// </summary>
        /// <param name="dto">Servis bilgileri</param>
        /// <returns>ClientId ve ClientSecret</returns>
        IDataResult<ServiceClientCreatedDto> CreateServiceClient(ServiceClientDto dto);
        
        /// <summary>
        /// ServiceClient'ı günceller
        /// </summary>
        /// <param name="clientId">Güncellenecek client</param>
        /// <param name="dto">Yeni bilgiler</param>
        IResult UpdateServiceClient(string clientId, ServiceClientDto dto);
        
        /// <summary>
        /// ServiceClient'ı deaktif eder
        /// </summary>
        /// <param name="clientId">Deaktif edilecek client</param>
        IResult DeactivateServiceClient(string clientId);
        
        /// <summary>
        /// ServiceClient için yeni secret oluşturur
        /// </summary>
        /// <param name="clientId">Client</param>
        /// <returns>Yeni ClientSecret</returns>
        IDataResult<ServiceClientCreatedDto> RegenerateClientSecret(string clientId);
    }
}
