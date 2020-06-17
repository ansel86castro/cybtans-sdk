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

        public IUnitOfWork UnitOfWork { get; }

        Type IQueryable.ElementType => typeof(T);

        Expression IQueryable.Expression => ((IQueryable)_dbSet).Expression;

        IQueryProvider IQueryable.Provider => ((IQueryable)_dbSet).Provider;

        public IQueryable<T> GetAll(ReadConsistency consistency = ReadConsistency.Default, Expression<Func<T, object>>[] include = null)
        {
            IQueryable<T> query = _dbSet;
            if(include != null)
            {
                foreach (var item in include)
                {
                   query = query.Include(item);
                }
            }

            return query.AsNoTracking();
        }

        public ValueTask<T> Get(TKey key, ReadConsistency consistency = ReadConsistency.Default)
        {
            return _dbSet.FindAsync(key);
        }
       

        public void Update(T item)
        {
            _dbSet.Update(item);         
        }

        public void UpdateRange(IEnumerable<T> items)
        {
            _dbSet.UpdateRange(items);
        }

        public void Add(T item)
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

        public void AddRange(IEnumerable<T> items)
        {
            _dbSet.AddRange(items);
        }

        public void Remove(T item)
        {          
            _dbSet.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {           
            _dbSet.RemoveRange(items);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)_dbSet).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dbSet).GetEnumerator();
        }      
    }

    public class EfRepository<T> : EfRepository<T, int>, IRepository<T>
        where T : class
    {
        public EfRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
