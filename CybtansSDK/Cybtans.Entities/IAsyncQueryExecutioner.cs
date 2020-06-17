using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities
{
    public interface IAsyncQueryExecutioner
    {
        public static IAsyncQueryExecutioner? Executioner { get; protected set; } = null;

        void SetCurrent();

        Task<List<T>> ToListAsync<T>(IQueryable<T> query);

        Task<T> FirstAsync<T>(IQueryable<T> query);

        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query);

        Task<T[]> ToArrayAsync<T>(IQueryable<T> query);

        Task<long> LongCountAsync<T>(IQueryable<T> query);

        Task<int> CountAsync<T>(IQueryable<T> query);
    }
}
