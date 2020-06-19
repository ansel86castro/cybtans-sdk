using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities
{

    public static class QueryableExtensions
    {       
        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.ToList());

            return IAsyncQueryExecutioner.Executioner.ToListAsync(query);
        }

        public static Task<T> FirstAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.First());

            return IAsyncQueryExecutioner.Executioner.FirstAsync(query);            
        }

        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.FirstOrDefault());

            return IAsyncQueryExecutioner.Executioner.FirstOrDefaultAsync(query);
        }

        public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.ToArray());

            return IAsyncQueryExecutioner.Executioner.ToArrayAsync(query);
        }

        public static Task<long> LongCountAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.LongCount());

            return IAsyncQueryExecutioner.Executioner.LongCountAsync(query);
        }

        public static Task<int> CountAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.Count());

            return IAsyncQueryExecutioner.Executioner.CountAsync(query);
        }
    }
}
