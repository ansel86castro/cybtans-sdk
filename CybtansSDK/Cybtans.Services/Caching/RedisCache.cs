using Cybtans.Serialization;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Services.Caching
{
    public class RedisCache : ICacheService
    {        
        readonly IConnectionMultiplexer _connection;
        readonly IDatabase _db;
        private LuaScript _preparedScript;

        public RedisCache(RedisConnectionProvider connectionProvider)
        {
            _connection = connectionProvider.GetConnection();
            _db = _connection.GetDatabase();


            var script = $@"
local i
i = redis.call('incrby', @Key, @Value)
redis.call('expire', @Key, @Expire) 
return i 
";
            _preparedScript = LuaScript.Prepare(script);
        }

        public async Task<T> Get<T>(string key)
            where T : class
        {
            RedisValue entry = await _db.StringGetAsync(key).ConfigureAwait(false);
            if (!entry.HasValue)
                return null;

            return BinaryConvert.Deserialize<T>(entry);
        }

        public async Task Set(string key, object item, TimeSpan? expire = null)
        {
            var bytes = BinaryConvert.Serialize(item);
            var result = await _db.StringSetAsync(key, bytes, expiry: expire).ConfigureAwait(false);
            if (!result)
                throw new InvalidOperationException("cache error");
        }

        public async Task<T> GetOrSet<T>(string key, Func<Task<T>> func, TimeSpan? expire = null)
            where T : class
        {
            T cacheEntry;
            RedisValue entry = await _db.StringGetAsync(key).ConfigureAwait(false);

            if (!entry.HasValue)
            {
                cacheEntry = await func();
                if (cacheEntry == null)
                    throw new InvalidOperationException($"Cache entry can not be null");

                var bytes = BinaryConvert.Serialize(cacheEntry);
                var result = await _db.StringSetAsync(key, bytes, expiry: expire).ConfigureAwait(false);
                if (!result)
                    throw new InvalidOperationException("cache error");

                return cacheEntry;
            }

            return BinaryConvert.Deserialize<T>(entry);
        }

        public Task Delete(string key)
        {
            return _db.KeyDeleteAsync(key);
        }

        public async Task<long> Increment(string key, long value = 1, TimeSpan? expire= null)
        {
            if (expire == null)
            {
                return await _db.StringIncrementAsync(key, value).ConfigureAwait(false);
            }

            RedisResult result = await _db.ScriptEvaluateAsync(_preparedScript, new { Key = new RedisKey(key), Value = value, Expire = expire.Value.TotalSeconds }).ConfigureAwait(false);

            return (long)result;
        }

        public async Task<long?> GetInteger(string key)
        {
            RedisValue entry = await _db.StringGetAsync(key).ConfigureAwait(false);
            if (!entry.HasValue)
                return null;

            return (long)entry;
        }

    }
}
