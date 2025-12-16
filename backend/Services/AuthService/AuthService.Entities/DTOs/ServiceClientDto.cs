using Core.Entities;

namespace AuthService.Entities.DTOs
{
    /// <summary>
    /// ServiceClient oluşturma/güncelleme DTO
    /// </summary>
    public class ServiceClientDto : IDto
    {
        public string ServiceName { get; set; } = string.Empty;
        public string AllowedScopes { get; set; } = string.Empty;
        public int TokenExpirationMinutes { get; set; } = 60;
    }

    /// <summary>
    /// ServiceClient oluşturulduktan sonra dönen response
    /// ClientSecret sadece bir kez gösterilir
    /// </summary>
    public class ServiceClientCreatedDto : IDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        
        /// <summary>
        /// DİKKAT: Bu değer sadece bir kez gösterilir!
        /// Güvenli bir şekilde saklanmalıdır.
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
        
        public string AllowedScopes { get; set; } = string.Empty;
        public int TokenExpirationMinutes { get; set; }
    }
}
