using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public class EfRepository<T, TKey> : IRepository<T, TKey>, IAsyncEnumerable<T> where T : class 
    {
        private readonly DbContext _context;
        private readonly DbContext _readOnlyContext;
        private readonly DbSet<T> _dbSet;
        private readonly DbSet<T> _readOnlydbSet;

        public EfRepository(IDbContextUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _context = unitOfWork.GetContext(ReadConsistency.Strong);
            _readOnlyContext = unitOfWork.GetContext(ReadConsistency.Weak);
            _dbSet = _context.Set<T>();
            _readOnlydbSet = _readOnlyContext == _context ? _dbSet : _readOnlyContext.Set<T>();
        }

        protected DbContext Context => _context;

        protected DbContext ReadOnlyContext => _readOnlyContext;

        protected DbSet<T> DbSet => _dbSet;

        protected DbSet<T> ReadOnlySet => _readOnlydbSet;

        public IUnitOfWork UnitOfWork { get; }

        Type IQueryable.ElementType => GetQueryable(ReadConsistency.Default).ElementType;

        Expression IQueryable.Expression => GetQueryable(ReadConsistency.Default).Expression;

        IQueryProvider IQueryable.Provider => GetQueryable(ReadConsistency.Default).Provider;

        protected virtual IQueryable<T> GetQueryable(ReadConsistency consistency)
        {
            switch (consistency)
            {
                case ReadConsistency.Strong: return _dbSet;
                case ReadConsistency.Weak: return _readOnlydbSet;
                default: return _dbSet;
            }
        }

        protected virtual IQueryable<T> WithDetails(ReadConsistency consistency) => GetQueryable(consistency);

        protected virtual IQueryable<T> WithDetailsSingle(ReadConsistency consistency) => WithDetails(consistency);

        private async ValueTask<T> GetWithDetails(IEntityKey<T> key, ReadConsistency consistency)
        {            
            return await WithDetailsSingle(consistency).FirstOrDefaultAsync(key.GetFilter());
        }

        private ValueTask<T> FindAsync(ReadConsistency consistency, params object[] keyValues)
        {
            if (consistency == ReadConsistency.Weak) return _readOnlydbSet.FindAsync(keyValues);
            return _dbSet.FindAsync(keyValues);
        }


        public Task<PagedQuery<T>> GetAllPaginated(
          int page = 1,
          int pageSize = 100,
          bool includeDetails = true,
          Func<IQueryable<T>, IQueryable<T>> filterFunc = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> sortFunc = null,
          ReadConsistency consistency = ReadConsistency.Default)
        {
            var query = GetAll(includeDetails, consistency);

            if (filterFunc != null)
                query = filterFunc(query);

            return query.AsPagedQuery(page, pageSize, sortFunc);
        }

        public virtual IQueryable<T> GetAll(bool includeDetails = false, ReadConsistency consistency = ReadConsistency.Default)
        {
            IQueryable<T> query = includeDetails ? WithDetails(consistency) : GetQueryable(consistency);

            return query.AsNoTracking();
        }

        public virtual ValueTask<T> Get(TKey key, bool includeDetails = false, ReadConsistency consistency = ReadConsistency.Default)
        {
            if(key is IEntityKey<T> ek)
            {
                return includeDetails ? GetWithDetails(ek, consistency) : FindAsync(consistency, ek.GetValues());
            }
            else if(key is ITuple tuple)
            {
                var ids = new object[tuple.Length];
                for (int i = 0; i < tuple.Length; i++)
                {
                    ids[i] = tuple[i];
                }

                return FindAsync(consistency, ids);
            }

            return FindAsync(consistency, key);
        }

        public virtual void Update(T item)
        {
            if (item is IAuditableEntity au && au.UpdateDate == null)
            {
                au.UpdateDate = DateTime.UtcNow;
            }
            _context.Update(item);
        }

        public virtual void UpdateRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (item is IAuditableEntity au && au.UpdateDate == null)
                {
                    au.UpdateDate = DateTime.UtcNow;
                }
            }

            _context.UpdateRange(items);
        }

        public virtual void Add(T item)
        {
            if (item is IAuditableEntity au)
            {
                if (au.Creator == null)
                {
                    var principal = Thread.CurrentPrincipal;
                    au.Creator = principal?.Identity?.Name;
                }
                au.CreateDate = DateTime.UtcNow;
            }

            _context.Add(item);
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (item is IAuditableEntity au)
                {
                    if (au.Creator == null)
                    {
                        var principal = Thread.CurrentPrincipal;
                        au.Creator = principal?.Identity?.Name;
                    }
                    au.CreateDate = DateTime.UtcNow;
                }
            }

            _context.AddRange(items);
        }

        public virtual void Remove(T item)
        {
            if (item is ISoftDelete sd)
            {
                sd.IsDeleted = true;                
                _context.Update(item);
                return;
            }

            _context.Remove(item);
        }

        public async ValueTask Remove(IEntityKey<T> key)
        {
            var item =  await _context.FindAsync<T>(key.GetValues());
            Remove(item);
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Remove(item);
            }
        }

        #region IEnumerable

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetQueryable(ReadConsistency.Default).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetQueryable(ReadConsistency.Default).GetEnumerator();
        }
      
        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return ((IAsyncEnumerable<T>)_dbSet).GetAsyncEnumerator(cancellationToken);
        }

        #endregion

        //protected IQueryable<T> WithDetails(params Expression<Func<T, object>>[] propertySelectors)
        //{
        //    IQueryable<T> query = GetQueryable();

        //    if (propertySelectors != null)
        //    {
        //        foreach (var item in propertySelectors)
        //        {
        //            query = query.Include(item);
        //        }
        //    }
        //    return query;
        //}

    }

    public class EfRepository<T> : EfRepository<T, object>, IRepository<T>
        where T : class
    {
        public EfRepository(IDbContextUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
