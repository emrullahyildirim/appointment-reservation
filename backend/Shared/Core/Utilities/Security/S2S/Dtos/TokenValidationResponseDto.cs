using Core.Entities;

namespace Core.Utilities.Security.S2S
{
    /// <summary>
    /// Token doğrulama response DTO
    /// </summary>
    public class TokenValidationResponseDto : IDto
    {
        /// <summary>
        /// Token geçerli mi?
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Token tipi: "user" veya "service"
        /// </summary>
        public string TokenType { get; set; } = string.Empty;

        /// <summary>
        /// User token ise UserId, Service token ise ServiceClientId
        /// </summary>
        public string? SubjectId { get; set; }

        /// <summary>
        /// User token için email, Service token için service adı
        /// </summary>
        public string? SubjectName { get; set; }

        /// <summary>
        /// Token'daki roller/scope'lar
        /// </summary>
        public List<string> Claims { get; set; } = new();

        /// <summary>
        /// Token bitiş zamanı
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Hata mesajı (token geçersizse)
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
