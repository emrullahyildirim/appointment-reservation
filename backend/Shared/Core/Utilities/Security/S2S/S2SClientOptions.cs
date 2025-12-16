namespace Core.Utilities.Security.S2S
{
    /// <summary>
    /// S2S Client yapılandırma ayarları
    /// Her servisin appsettings.json'ında tanımlanmalı
    /// </summary>
    public class S2SClientOptions
    {
        /// <summary>
        /// AuthService'in base URL'i
        /// Örn: "https://localhost:5001" veya "http://auth-service"
        /// </summary>
        public string AuthServiceUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// Bu servisin ClientId'si
        /// </summary>
        public string ClientId { get; set; } = string.Empty;
        
        /// <summary>
        /// Bu servisin ClientSecret'ı
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
        
        /// <summary>
        /// Bu servisin erişebileceği scope'lar (opsiyonel)
        /// Belirtilmezse, tüm izinli scope'lar için token alınır
        /// </summary>
        public string? DefaultScopes { get; set; }
        
        /// <summary>
        /// Token cache süresi (dakika)
        /// Token'ın expire süresinden bu kadar önce yenilenecek
        /// </summary>
        public int TokenRefreshBufferMinutes { get; set; } = 5;
    }
}
