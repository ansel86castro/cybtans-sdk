using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public class EfAsyncQueryExecutioner : IAsyncQueryExecutioner
    {
        public void SetCurrent()
        {
            IAsyncQueryExecutioner.Executioner = this;
        }


        public static void Setup()
        {
            IAsyncQueryExecutioner.Executioner = new EfAsyncQueryExecutioner();
        }    

        public Task<int> CountAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.CountAsync(query);
        }

        public Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            return EntityFrameworkQueryableExtensions.CountAsync(query, predicate);
        }

        public Task<T> FirstAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.FirstAsync(query);
        }

        public Task<T> FirstAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            return EntityFrameworkQueryableExtensions.FirstAsync(query, predicate);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, predicate);
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.LongCountAsync(query);
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            return EntityFrameworkQueryableExtensions.LongCountAsync(query, predicate);
        }
      
        public Task<TResult> MaxAsync<TSource, TResult>(IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return EntityFrameworkQueryableExtensions.MaxAsync(source, selector);
        }

        public Task<TResult> MinAsync<TSource, TResult>(IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return EntityFrameworkQueryableExtensions.MinAsync(source, selector);
        }

      
        public Task<T[]> ToArrayAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.ToArrayAsync(query);
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.ToListAsync(query);
        }

        public Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(source, selector);
        }

        public Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(source, selector);
        }

        public Task<decimal> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(source, selector);
        }

        public Task<decimal?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(source, selector);
        }

        public Task<decimal?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            return EntityFrameworkQueryableExtensions.SumAsync(source, selector);
        }

        public Task<double> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            return EntityFrameworkQueryableExtensions.SumAsync(source, selector);
        }

        public Task<int> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return EntityFrameworkQueryableExtensions.SumAsync(source, selector);
        }

        public Task<double?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            return EntityFrameworkQueryableExtensions.SumAsync(source, selector);
        }

        public Task<int?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            return EntityFrameworkQueryableExtensions.SumAsync(source, selector);
        }

        public Task<long?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            return EntityFrameworkQueryableExtensions.SumAsync(source, selector);
        }

        public Task<long> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            return EntityFrameworkQueryableExtensions.SumAsync(source, selector);
        }

        public Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(source, selector);
        }

        public Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(source, selector);
        }

        public Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(source, selector);
        }

        public Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(source, selector);
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            return EntityFrameworkQueryableExtensions.AnyAsync(query, predicate);
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> query)
        {
            return EntityFrameworkQueryableExtensions.AnyAsync(query);
        }

        public Task<bool> AllAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            return EntityFrameworkQueryableExtensions.AllAsync(query, predicate);
        }

        public Task<Dictionary<TKey, T>> ToDictionaryAsync<T,TKey>(IQueryable<T> query, Func<T,TKey> keySelector)
        {
            return EntityFrameworkQueryableExtensions.ToDictionaryAsync(query, keySelector);
        }

        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<T, TKey, TElement>(IQueryable<T> query, Func<T, TKey> keySelector, Func<T, TElement> elementSelector)
        {
            return EntityFrameworkQueryableExtensions.ToDictionaryAsync(query, keySelector, elementSelector);
        }

        public IQueryable<T> AsNoTracking<T>(IQueryable<T> query)
            where T:class
        {
            return EntityFrameworkQueryableExtensions.AsNoTracking(query);
        }

        public IIncludableQueryable<T, TProperty> Include<T, TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> navigationPropertyPath)
           where T : class
        {            
            return new EfIncludableQuerable<T, TProperty>(EntityFrameworkQueryableExtensions.Include(query, navigationPropertyPath));
        }

        public IIncludableQueryable<T, TProperty> ThenInclude<T, TPreviusProperty, TProperty>(IIncludableQueryable<T, IEnumerable<TPreviusProperty>> query, Expression<Func<TPreviusProperty, TProperty>> navigationPropertyPath)
          where T : class
        {
            var efQuery = ((EfIncludableQuerable<T, IEnumerable<TPreviusProperty>>)query).EfQuery;

            return new EfIncludableQuerable<T, TProperty>(EntityFrameworkQueryableExtensions.ThenInclude(efQuery, navigationPropertyPath));
        }

    }
}
