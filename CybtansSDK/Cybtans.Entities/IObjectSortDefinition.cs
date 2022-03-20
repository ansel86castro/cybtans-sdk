using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cybtans.Entities
{
    public interface ISortDefinitionBuilder<T>
    {
        IObjectSortDefinition<T> Ascending(Expression<Func<T, object>> prop);

        IObjectSortDefinition<T> Descending(Expression<Func<T, object>> prop);

        IObjectSortDefinition<T> Combine(params IObjectSortDefinition<T>[] sorts);

        IObjectSortDefinition<T> Combine(IEnumerable<IObjectSortDefinition<T>> sorts);

    }

    public interface IObjectSortDefinition<T>
    {

    }
}
