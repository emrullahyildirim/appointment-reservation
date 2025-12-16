using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.RegularExpressions;

namespace Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly RedisOptions _options;
        

        public RedisCacheManager(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> options)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _options = options.Value;
            _database = _connectionMultiplexer.GetDatabase(_options.DefaultDatabase);
        }

        public void Add(string key, object value, int duration)
        {
            var serializedValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            });

            var prefixedKey = GetPrefixedKey(key);
            _database.StringSet(prefixedKey, serializedValue, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            var value = _database.StringGet(prefixedKey);

            if (value.IsNullOrEmpty)
                return default!;

            return JsonConvert.DeserializeObject<T>(value!, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            })!;
        }

        public object Get(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            var value = _database.StringGet(prefixedKey);

            if (value.IsNullOrEmpty)
                return null!;

            return JsonConvert.DeserializeObject(value!, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            })!;
        }

        public bool IsAdd(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            return _database.KeyExists(prefixedKey);
        }

        public void Remove(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            _database.KeyDelete(prefixedKey);
        }

        public void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var endpoints = _connectionMultiplexer.GetEndPoints();

            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(database: _options.DefaultDatabase, pattern: $"{_options.InstanceName}*");

                foreach (var key in keys)
                {
                    var keyWithoutPrefix = key.ToString().Replace($"{_options.InstanceName}", "");
                    if (regex.IsMatch(keyWithoutPrefix))
                    {
                        _database.KeyDelete(key);
                    }
                }
            }
        }

        private string GetPrefixedKey(string key)
        {
            return $"{_options.InstanceName}{key}";
        }
    }
}

