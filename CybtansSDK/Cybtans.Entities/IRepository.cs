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

    public interface IRepository<T, TKey>
    {
        IQueryable<T> GetAll(ReadConsistency consistency = ReadConsistency.Default, Expression<Func<T, object>>[] include = null);

        ValueTask<T> Load(TKey key, ReadConsistency consistency = ReadConsistency.Default);

        void Update(T item);

        void Create(T item);

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
