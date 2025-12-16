using Core.Entities;

namespace Core.Utilities.Security.S2S
{
    /// <summary>
    /// OAuth 2.0 Client Credentials Grant için request DTO
    /// Servisler arası iletişimde kullanılır
    /// </summary>
    public class S2STokenRequestDto : IDto
    {
        /// <summary>
        /// Servisin ClientId'si
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Servisin ClientSecret'ı
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// Hedef servis scope'ları (virgülle ayrılmış)
        /// Örn: "CategoryService" veya "CategoryService,InventoryService"
        /// </summary>
        public string? Scope { get; set; }
    }
}
