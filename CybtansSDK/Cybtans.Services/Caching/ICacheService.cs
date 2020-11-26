using System;
using System.Threading.Tasks;

namespace Cybtans.Services.Caching
{
    public interface ICacheService
    {        
        Task Delete(string key);
        Task<T> Get<T>(string key) where T : class;
        Task<T> GetOrSet<T>(string key, Func<Task<T>> func, TimeSpan? expire = null) where T : class;        
        Task Set(string key, object item, TimeSpan? expire = null);
        Task<long> Increment(string key, long value = 1, TimeSpan? expire = null);
        Task<long?> GetInteger(string key);
        public Task<long> Decrement(string key, long value = 1, TimeSpan? expire = null) => Increment(key, -value, expire);
    }
}