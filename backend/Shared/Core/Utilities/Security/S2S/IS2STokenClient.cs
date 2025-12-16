namespace Core.Utilities.Security.S2S
{
    /// <summary>
    /// S2S Token Client interface
    /// AuthService'den token almak için kullanılır
    /// Kubernetes multi-pod ortamında distributed cache (Redis) ile çalışır
    /// </summary>
    public interface IS2STokenClient
    {
        /// <summary>
        /// Belirtilen scope için S2S token alır
        /// Token distributed cache'e kaydedilir ve expire olmadan önce yenilenir
        /// </summary>
        /// <param name="scope">Hedef servis scope'u (örn: "CategoryService")</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Bearer token</returns>
        Task<string> GetTokenAsync(string? scope = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Belirtilen scope için cache'lenmiş token'ı temizler
        /// Tüm pod'larda geçerlidir (distributed cache)
        /// </summary>
        /// <param name="scope">Temizlenecek scope (null ise default scope)</param>
        void InvalidateToken(string? scope = null);
        
        /// <summary>
        /// Bu client için tüm cache'lenmiş token'ları temizler
        /// Tüm pod'larda geçerlidir (distributed cache)
        /// </summary>
        void InvalidateAllTokens();
    }
}
