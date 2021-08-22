using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities
{
    public interface IObjectRepository<T> : IAsyncEnumerable<T>
    {
        Task<T> Get(Expression<Func<T, bool>> filter, ReadConsistency consistency = ReadConsistency.Default);
        Task<PagedList<T>> GetManyAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? sortBy = null);
        IAsyncEnumerator<T> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? sortBy = null);
        Task<T> AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        Task<long> UpdateAsync(Expression<Func<T, bool>> filter, IDictionary<string, object> data);
        Task<long> UpdateAsync(Expression<Func<T, bool>> filter, T item);
        Task<long> DeleteAsync(Expression<Func<T, bool>> filter);

        #region Defaults

        public async Task<List<T>> ToListAsync()
        {
            var list = new List<T>();
            await foreach(var item in this)
            {
                list.Add(item);
            }
            return list;
        }

        public async Task<HashSet<T>> ToHashSetAsync()
        {
            var list = new HashSet<T>();
            await foreach (var item in this)
            {
                list.Add(item);
            }
            return list;
        }

        public async Task<Dictionary<TKey,T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector)
        {
            var dic = new Dictionary<TKey, T>();
            await foreach (var item in this)
            {
                dic.Add(keySelector(item), item);
            }
            return dic;
        }

        public async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector)
        {
            var dic = new Dictionary<TKey, TElement>();
            await foreach (var item in this)
            {
                dic.Add(keySelector(item), elementSelector(item));
            }
            return dic;
        }

        #endregion
    }

    public interface IObjectRepository<T, TKey> : IObjectRepository<T>
        where T : IEntity<TKey>
    {
        Task<T> Get(TKey id, ReadConsistency consistency = ReadConsistency.Default);

        Task<long> UpdateAsync(TKey id, IDictionary<string, object> data);

        Task<long> UpdateAsync(TKey id, T item);

        Task<long> DeleteAsync(TKey id);
    }
}
