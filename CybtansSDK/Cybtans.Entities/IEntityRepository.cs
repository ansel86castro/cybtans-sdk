using System;

namespace Cybtans.Entities
{
    public interface IEntityRepository<T, TKey> : IRepository<T, TKey>
        where T: IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

    }

}
