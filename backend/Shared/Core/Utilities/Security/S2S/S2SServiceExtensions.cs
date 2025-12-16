using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Utilities.Security.S2S
{
    /// <summary>
    /// S2S servis extension'ları
    /// </summary>
    public static class S2SServiceExtensions
    {
        /// <summary>
        /// S2S Token Client'ı DI container'a ekler
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="configSection">Config section adı (default: "S2SClient")</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddS2STokenClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSection = "S2SClient")
        {
            services.Configure<S2SClientOptions>(configuration.GetSection(configSection));
            
            services.AddHttpClient<IS2STokenClient, S2STokenClient>();
            
            return services;
        }

        /// <summary>
        /// S2S token ile korunan bir HttpClient ekler
        /// </summary>
        /// <typeparam name="TClient">HttpClient tipi (interface)</typeparam>
        /// <typeparam name="TImplementation">HttpClient implementasyonu</typeparam>
        /// <param name="services">Service collection</param>
        /// <param name="baseAddress">Hedef servisin base adresi</param>
        /// <param name="scope">İstenen scope (opsiyonel)</param>
        /// <returns>IHttpClientBuilder</returns>
        public static IHttpClientBuilder AddS2SHttpClient<TClient, TImplementation>(
            this IServiceCollection services,
            string baseAddress,
            string? scope = null)
            where TClient : class
            where TImplementation : class, TClient
        {
            return services.AddHttpClient<TClient, TImplementation>(client =>
            {
                client.BaseAddress = new Uri(baseAddress.TrimEnd('/') + "/");
            })
            .AddHttpMessageHandler(sp =>
            {
                var tokenClient = sp.GetRequiredService<IS2STokenClient>();
                return new S2STokenHandler(tokenClient, scope);
            });
        }

        /// <summary>
        /// S2S token ile korunan named HttpClient ekler
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="name">HttpClient adı</param>
        /// <param name="baseAddress">Hedef servisin base adresi</param>
        /// <param name="scope">İstenen scope (opsiyonel)</param>
        /// <returns>IHttpClientBuilder</returns>
        public static IHttpClientBuilder AddS2SHttpClient(
            this IServiceCollection services,
            string name,
            string baseAddress,
            string? scope = null)
        {
            return services.AddHttpClient(name, client =>
            {
                client.BaseAddress = new Uri(baseAddress.TrimEnd('/') + "/");
            })
            .AddHttpMessageHandler(sp =>
            {
                var tokenClient = sp.GetRequiredService<IS2STokenClient>();
                return new S2STokenHandler(tokenClient, scope);
            });
        }
    }
}
