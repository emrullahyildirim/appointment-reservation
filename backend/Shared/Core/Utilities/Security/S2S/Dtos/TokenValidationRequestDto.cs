using Core.Entities;

namespace Core.Utilities.Security.S2S
{
    /// <summary>
    /// Token doğrulama request DTO
    /// Gateway veya diğer servisler token'ı doğrulamak için kullanır
    /// </summary>
    public class TokenValidationRequestDto : IDto
    {
        /// <summary>
        /// Doğrulanacak token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Beklenen scope (opsiyonel)
        /// Eğer belirtilirse, token'ın bu scope'a sahip olup olmadığı kontrol edilir
        /// </summary>
        public string? RequiredScope { get; set; }
    }
}
