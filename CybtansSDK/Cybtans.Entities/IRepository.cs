using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cybtans.Entities
{
    public enum ReadConsistency
    {
        Default,
        Weak,
        Strong
    }

    public interface IRepository<T, TKey> :IQueryable<T>
    {
        IQueryable<T> GetAll(ReadConsistency consistency = ReadConsistency.Default, Expression<Func<T, object>>[] include = null);

        ValueTask<T> Get(TKey key, ReadConsistency consistency = ReadConsistency.Default);

        void Update(T item);

        void UpdateRange(IEnumerable<T> items);

        void Add(T item);

        void AddRange(IEnumerable<T> items);

        void Remove(T item);

        void RemoveRange(IEnumerable<T> item);

        IUnitOfWork UnitOfWork { get; }
    }

    public interface IRepository<T>:IRepository<T, int>
    {

    }

    public interface IEntityRepository<T>:IRepository<T, Guid>
    {

    }
}
