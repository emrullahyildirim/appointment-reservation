using Core.Entities;

namespace Core.Utilities.Security.S2S
{
    /// <summary>
    /// S2S Token response DTO
    /// OAuth 2.0 Client Credentials Grant response formatında
    /// </summary>
    public class S2STokenResponseDto : IDto
    {
        /// <summary>
        /// JWT Access Token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Token tipi (her zaman "Bearer")
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Token geçerlilik süresi (saniye)
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Token'ın son geçerlilik tarihi
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Verilen scope'lar
        /// </summary>
        public string Scope { get; set; } = string.Empty;
    }
}
