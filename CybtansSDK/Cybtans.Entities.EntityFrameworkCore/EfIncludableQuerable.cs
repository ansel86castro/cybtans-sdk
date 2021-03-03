using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public interface IEfIncludableQuerable<out TEntity, out  TProperty> : IIncludableQueryable<TEntity, TProperty>, IAsyncEnumerable<TEntity> where TEntity : class
    {
        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty> EfQuery { get; }
    }

    public class EfIncludableQuerable<TEntity, TProperty> : IEfIncludableQuerable<TEntity, TProperty> where TEntity : class
    {
        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty> _query;

        public EfIncludableQuerable(Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty> query)
        {
            _query = query;
        }

        public Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty> EfQuery => _query;

        public Type ElementType => _query.ElementType;
        public Expression Expression => _query.Expression;
        public IQueryProvider Provider => _query.Provider;

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _query.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _query.GetEnumerator();
        }

        IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return ((IAsyncEnumerable<TEntity>)_query).GetAsyncEnumerator(cancellationToken);
        }
    }
}
