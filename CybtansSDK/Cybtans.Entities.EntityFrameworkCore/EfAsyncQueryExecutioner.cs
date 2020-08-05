using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public class EfAsyncQueryExecutioner : IAsyncQueryExecutioner
    {
        public static void Setup()
        {
            IAsyncQueryExecutioner.Executioner = new EfAsyncQueryExecutioner();
        }

        public Task<int> CountAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.CountAsync(query);
        }

        public Task<T> FirstAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.FirstAsync(query);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query);
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.LongCountAsync(query);
        }

        public void SetCurrent()
        {
            IAsyncQueryExecutioner.Executioner = this;
        }

        public Task<T[]> ToArrayAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.ToArrayAsync(query);
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.ToListAsync(query);
        }
    }
}
