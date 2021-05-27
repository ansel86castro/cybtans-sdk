using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Entities
{
    public enum ReadConsistency
    {
        Strong = 0,
        Weak = 1,
        Default = Strong
    }

    public interface IRepository<T, TKey> :IQueryable<T>
    {
        Task<PagedQuery<T>> GetAllPaginated(
          int page = 1,
          int pageSize = 100,
          bool includeDetails = true,
          Func<IQueryable<T>, IQueryable<T>> filterFunc = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> sortFunc = null,          
          ReadConsistency consistency = ReadConsistency.Default);

        IQueryable<T> GetAll(bool includeDetails = false, ReadConsistency consistency = ReadConsistency.Default);

        ValueTask<T> Get(TKey key, bool includeDetails = false, ReadConsistency consistency = ReadConsistency.Default);

        void Update(T item);

        void UpdateRange(IEnumerable<T> items);

        void Add(T item);

        void AddRange(IEnumerable<T> items);

        void Remove(T item);

        void RemoveRange(IEnumerable<T> item);

        IUnitOfWork UnitOfWork { get; }

        public Task SaveChangesAsync()
        {
            return UnitOfWork.SaveChangesAsync();
        }
    }

    public interface IRepository<T>:IRepository<T, object>
    {

    }

}
