using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.Extensions
{
    public static class RedisServiceExtensions
    {
        public static IServiceCollection AddRedisCache(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = "RedisOptions")
        {
            var redisOptions = configuration
                .GetSection(sectionName)
                .Get<RedisOptions>() ?? new RedisOptions();

            services.Configure<RedisOptions>(
                configuration.GetSection(sectionName));

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { redisOptions.ConnectionString },
                AbortOnConnectFail = redisOptions.AbortOnConnectFail,
                ConnectTimeout = redisOptions.ConnectTimeout,
                SyncTimeout = redisOptions.SyncTimeout,
                DefaultDatabase = redisOptions.DefaultDatabase
            };

            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(configurationOptions));

            services.AddSingleton<ICacheManager, RedisCacheManager>();

            return services;
        }
    }
}
