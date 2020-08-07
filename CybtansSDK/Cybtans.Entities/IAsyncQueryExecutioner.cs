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

        Task<int> CountAsync<T>(IQueryable<T> query);
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
    }
}
