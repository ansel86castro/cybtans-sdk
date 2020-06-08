using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntiyFrameworkCore
{
    public class DbSetRepository<T, TKey> : IRepository<T, TKey> where T : class       
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public DbSetRepository(DbContext context, IUnitOfWork unitOfWork)
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

        public ValueTask<T> Load(TKey key, ReadConsistency consistency = ReadConsistency.Default)
        {
            return _dbSet.FindAsync(key);
        }
       

        public void Update(T item)
        {
            var entry = _context.Entry(item);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(item);
                entry.State = EntityState.Modified;                
            }
            else if (entry.State == EntityState.Unchanged)
            {
                entry.State = EntityState.Modified;                
            }
        }

        public void Create(T item)
        {
            _dbSet.Add(item);
        }

        public void Remove(T item)
        {
            var entry = _context.Entry(item);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(item);
            }
            _dbSet.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                var entry = _context.Entry(item);
                if (entry.State == EntityState.Detached)
                {
                    _dbSet.Attach(item);
                }
            }

            _dbSet.RemoveRange(items);
        }

    }
}
