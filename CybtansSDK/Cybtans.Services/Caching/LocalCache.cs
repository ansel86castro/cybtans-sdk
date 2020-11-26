using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Services.Caching
{
    public class LocalCache : ICacheService
    {
        MemoryCache _cache;

        SpinLock spinLock = new SpinLock();

        public LocalCache()
        {
            var options = Options.Create(new MemoryCacheOptions { });
            _cache = new MemoryCache(options);            
        }

        public Task<T> GetOrSet<T>(string key, Func<Task<T>> func, TimeSpan? expire = null) where T : class
        {
            return _cache.GetOrCreateAsync(key, e =>
            {
                e.AbsoluteExpirationRelativeToNow = expire;
                return func();
            });
        }

        public Task<long> Increment(string key, long value = 1, TimeSpan? expire= null)
        {
            bool taken = false;
            try
            {
                spinLock.Enter(ref taken);

                if (!_cache.TryGetValue(key, out object result))
                {
                    result = 0L;
                    var entry = _cache.CreateEntry(key);                    
                    entry.SetValue(result);
                    if (expire != null)
                        entry.SetAbsoluteExpiration(expire.Value);
                    
                    entry.Dispose();
                }

                var n = (long)result;
                n += value;

                _cache.Set(key, n);
                return Task.FromResult<long>(n);
            }
            finally
            {
                if (taken)
                {
                    spinLock.Exit();
                }
            }
        }

      
        public Task Delete(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task<T> Get<T>(string key) where T : class
        {
            return Task.FromResult(_cache.Get<T>(key));
        }

        public Task Set(string key, object item, TimeSpan? expire = null)
        {          
            var entry = _cache.CreateEntry(key);            
            entry.SetValue(item);
            if (expire != null)
                entry.SetAbsoluteExpiration(expire.Value);

            entry.Dispose();

            return Task.CompletedTask;
        }

        public Task<long?> GetInteger(string key)
        {
            if (_cache.TryGetValue(key, out var l))
            {
                return Task.FromResult((long?)Convert.ToInt64(l));
            }
            return Task.FromResult((long?)null);
        }
    }
}
