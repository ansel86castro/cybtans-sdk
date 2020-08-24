using Cybtans.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntiyFrameworkCore
{
    public class EfRepository<T, TKey> : IRepository<T, TKey> where T : class       
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;      
        
        public EfRepository(IUnitOfWork unitOfWork)
        {                        
            UnitOfWork = unitOfWork;
            _context = ((EfUnitOfWork)unitOfWork).Context;
            _dbSet = _context.Set<T>();
        }

        protected DbContext Context => _context;

        protected DbSet<T> DbSet => _dbSet;

        public IUnitOfWork UnitOfWork { get; }

        Type IQueryable.ElementType => GetQueryable().ElementType;

        Expression IQueryable.Expression => GetQueryable().Expression;

        IQueryProvider IQueryable.Provider => GetQueryable().Provider;

        protected virtual IQueryable<T> GetQueryable() => _dbSet;

        public virtual IQueryable<T> WithDetails()
        {
            return GetQueryable();
        }

        public virtual IQueryable<T> WithDetails(params Expression<Func<T, object>>[] propertySelectors)
        {            
            IQueryable<T> query = GetQueryable();

            if (propertySelectors != null)
            {
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            }
            return query;
        }


        public virtual IQueryable<T> GetAll(ReadConsistency consistency = ReadConsistency.Default, Expression<Func<T, object>>[] propertySelectors = null)
        {
            IQueryable<T> query = propertySelectors != null ? 
                WithDetails(propertySelectors) : 
                GetQueryable();       
            
            return query.AsNoTracking();
        }

        public virtual ValueTask<T> Get(TKey key, ReadConsistency consistency = ReadConsistency.Default)
        {                       
            return _dbSet.FindAsync(key);
        }
       
        public virtual void Update(T item)
        {
            if (item is IAuditableEntity au)
            {              
                au.UpdateDate = DateTime.UtcNow;
            }
            _dbSet.Update(item);         
        }

        public virtual void UpdateRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (item is IAuditableEntity au)
                {
                    au.UpdateDate = DateTime.UtcNow;
                }
            }
            _dbSet.UpdateRange(items);
        }

        public virtual void Add(T item)
        {
            if(item is IAuditableEntity au)
            {
                var principal = Thread.CurrentPrincipal;
                if(principal?.Identity?.Name != null)
                {
                    au.Creator = principal?.Identity?.Name;
                }
                au.CreateDate = DateTime.UtcNow;
            }

            _dbSet.Add(item);
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (item is IAuditableEntity au)
                {
                    var principal = Thread.CurrentPrincipal;
                    if (principal?.Identity?.Name != null)
                    {
                        au.Creator = principal?.Identity?.Name;
                    }
                    au.CreateDate = DateTime.UtcNow;
                }
            }
            _dbSet.AddRange(items);
        }

        public virtual void Remove(T item)
        {          
            _dbSet.Remove(item);
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {           
            _dbSet.RemoveRange(items);
        }
     

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetQueryable().GetEnumerator();
        }      
    }

    public class EfRepository<T> : EfRepository<T, object>, IRepository<T>
        where T : class
    {
        public EfRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
