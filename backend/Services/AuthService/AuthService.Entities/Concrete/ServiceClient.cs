using Core.Entities;

namespace AuthService.Entities.Concrete
{
    /// <summary>
    /// Mikroservis kimlik bilgilerini tutan entity.
    /// Her servis kendi ClientId ve ClientSecret ile S2S token alabilir.
    /// </summary>
    public class ServiceClient : IEntity
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Servis adı (örn: ProductService, CategoryService)
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;
        
        /// <summary>
        /// OAuth 2.0 Client Credentials için ClientId
        /// </summary>
        public string ClientId { get; set; } = string.Empty;
        
        /// <summary>
        /// Hashlenmiş ClientSecret
        /// </summary>
        public byte[] ClientSecretHash { get; set; } = Array.Empty<byte>();
        
        /// <summary>
        /// ClientSecret için salt
        /// </summary>
        public byte[] ClientSecretSalt { get; set; } = Array.Empty<byte>();
        
        /// <summary>
        /// Servisin aktif olup olmadığı
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Bu servisin hangi servislere erişebileceği (virgülle ayrılmış)
        /// Örn: "CategoryService,InventoryService"
        /// </summary>
        public string AllowedScopes { get; set; } = string.Empty;
        
        /// <summary>
        /// Servisin tanımlandığı tarih
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Son güncelleme tarihi
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        
        /// <summary>
        /// Token son kullanım süresi (dakika)
        /// </summary>
        public int TokenExpirationMinutes { get; set; } = 60;
    }
}
