using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities
{
    public interface IAsyncQueryExecutioner
    {
        public static IAsyncQueryExecutioner? Executioner { get; protected set; } = null;

        void SetCurrent();

        Task<List<T>> ToListAsync<T>(IQueryable<T> query);
        Task<T> FirstAsync<T>(IQueryable<T> query);
        Task<T> FirstAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query);
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate);
        Task<T[]> ToArrayAsync<T>(IQueryable<T> query);
        Task<long> LongCountAsync<T>(IQueryable<T> query);
        Task<long> LongCountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate);
        Task<int> CountAsync<T>(IQueryable<T> query);
        Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate);
        Task<TResult> MaxAsync<TSource, TResult>(IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector);
        Task<TResult> MinAsync<TSource, TResult>(IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector);
        Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector);
        Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector);
        Task<decimal> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector);
        Task<decimal?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector);
        Task<decimal?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector);
        Task<double> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector);
        Task<int> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector);
        Task<double?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector);
        Task<int?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector);
        Task<long?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector);
        Task<long> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector);
        Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector);
        Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector);
        Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector);
        Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector);
        Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync<T>(IQueryable<T> query);
        Task<bool> AllAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate);
        Task<Dictionary<TKey, T>> ToDictionaryAsync<T, TKey>(IQueryable<T> query, Func<T, TKey> keySelector);
        Task<Dictionary<TKey, TElement>> ToDictionaryAsync<T, TKey, TElement>(IQueryable<T> query, Func<T, TKey> keySelector, Func<T, TElement> elementSelector);
        IQueryable<T> AsNoTracking<T>(IQueryable<T> query) where T : class;
        IIncludableQueryable<T, TProperty> Include<T, TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> navigationPropertyPath) where T : class;
        IIncludableQueryable<T, TProperty> ThenInclude<T, TPreviusProperty, TProperty>(IIncludableQueryable<T, IEnumerable<TPreviusProperty>> query, Expression<Func<TPreviusProperty, TProperty>> navigationPropertyPath)
            where T : class;
    }
}
