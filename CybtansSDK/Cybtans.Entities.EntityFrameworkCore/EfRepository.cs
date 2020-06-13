using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntiyFrameworkCore
{
    public class EfRepository<T, TKey> : IRepository<T, TKey> where T : class       
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(DbContext context, IUnitOfWork unitOfWork)
        {            
            UnitOfWork = unitOfWork;
            _context = context;
            _dbSet = context.Set<T>();

            var model = context.Model.GetEntityTypes(typeof(T)).FirstOrDefault();
            var key = model.FindPrimaryKey();
            if (key == null)
                throw new InvalidOperationException("Entity does not have a key");
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

    public class DbSetRepository<T> : EfRepository<T, int>, IRepository<T>
        where T : class
    {
        public DbSetRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
