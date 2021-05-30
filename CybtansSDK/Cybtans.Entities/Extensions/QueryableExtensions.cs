using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities.Extensions
{

    public static class QueryableExtensions
    {
        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.ToList());

            return IAsyncQueryExecutioner.Executioner.ToListAsync(query);
        }

        public static Task<T> FirstAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.First());

            return IAsyncQueryExecutioner.Executioner.FirstAsync(query);
        }

        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.FirstOrDefault());

            return IAsyncQueryExecutioner.Executioner.FirstOrDefaultAsync(query);
        }

        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.FirstOrDefault(predicate));

            return IAsyncQueryExecutioner.Executioner.FirstOrDefaultAsync(query, predicate);
        }

        public static Task<T> FirstAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.First(predicate));

            return IAsyncQueryExecutioner.Executioner.FirstAsync(query, predicate);
        }

        public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.ToArray());

            return IAsyncQueryExecutioner.Executioner.ToArrayAsync(query);
        }

        public static Task<long> LongCountAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.LongCount());

            return IAsyncQueryExecutioner.Executioner.LongCountAsync(query);
        }

        public static Task<int> CountAsync<T>(this IQueryable<T> query)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.Count());

            return IAsyncQueryExecutioner.Executioner.CountAsync(query);
        }

        public static Task<TResult> MaxAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Max(selector));

            return IAsyncQueryExecutioner.Executioner.MaxAsync(source, selector);
        }

        public static Task<TResult> MinAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Min(selector));

            return IAsyncQueryExecutioner.Executioner.MinAsync(source, selector);
        }

        public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Average(selector));

            return IAsyncQueryExecutioner.Executioner.AverageAsync(source, selector);
        }

        public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Average(selector));

            return IAsyncQueryExecutioner.Executioner.AverageAsync(source, selector);
        }

        public static Task<decimal> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Average(selector));

            return IAsyncQueryExecutioner.Executioner.AverageAsync(source, selector);
        }

        public static Task<decimal?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Average(selector));

            return IAsyncQueryExecutioner.Executioner.AverageAsync(source, selector);
        }

        public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Average(selector));

            return IAsyncQueryExecutioner.Executioner.AverageAsync(source, selector);
        }

        public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Average(selector));

            return IAsyncQueryExecutioner.Executioner.AverageAsync(source, selector);
        }

        public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Average(selector));

            return IAsyncQueryExecutioner.Executioner.AverageAsync(source, selector);
        }

        public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Average(selector));

            return IAsyncQueryExecutioner.Executioner.AverageAsync(source, selector);
        }

        public static Task<decimal?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Sum(selector));

            return IAsyncQueryExecutioner.Executioner.SumAsync(source, selector);
        }

        public static Task<double> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Sum(selector));

            return IAsyncQueryExecutioner.Executioner.SumAsync(source, selector);
        }

        public static Task<double?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Sum(selector));

            return IAsyncQueryExecutioner.Executioner.SumAsync(source, selector);
        }

        public static Task<int> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Sum(selector));

            return IAsyncQueryExecutioner.Executioner.SumAsync(source, selector);
        }

        public static Task<int?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Sum(selector));

            return IAsyncQueryExecutioner.Executioner.SumAsync(source, selector);
        }

        public static Task<long> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Sum(selector));

            return IAsyncQueryExecutioner.Executioner.SumAsync(source, selector);
        }

        public static Task<long?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Sum(selector));

            return IAsyncQueryExecutioner.Executioner.SumAsync(source, selector);
        }

        public static PagedQuery<T> PageBy<T>(this IQueryable<T> source, int count, int skip = 0, int take = 50)
        {
            return new PagedQuery<T>(
                query: source.Skip(skip).Take(take),
                page: skip / take,
                totalPages: count / take + (count % take == 0 ? 0 : 1),
                totalCount: count);
        }

        public static async Task<PagedQuery<T>> PageBy<T>(this IQueryable<T> source, Func<IQueryable<T>, IQueryable<T>> filter, int skip = 0, int take = 50)
        {
            source = filter(source);
            var count = await source.CountAsync();
            return source.PageBy(count, skip, take);

        }


        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Any(predicate));

            return IAsyncQueryExecutioner.Executioner.AnyAsync(source, predicate);
        }

        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.Any());

            return IAsyncQueryExecutioner.Executioner.AnyAsync(source);
        }

        public static Task<bool> AllAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(source.All(predicate));

            return IAsyncQueryExecutioner.Executioner.AllAsync(source, predicate);
        }


        public static Task<int> CountAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.Count(predicate));

            return IAsyncQueryExecutioner.Executioner.CountAsync(query, predicate);
        }

        public static Task<long> LongCountAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.LongCount(predicate));

            return IAsyncQueryExecutioner.Executioner.LongCountAsync(query, predicate);
        }

        public static Task<Dictionary<TKey, T>> ToDictionaryAsync<T, TKey>(this IQueryable<T> query, Func<T, TKey> keySelector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.ToDictionary(keySelector));

            return IAsyncQueryExecutioner.Executioner.ToDictionaryAsync(query, keySelector);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<T, TKey, TElement>(this IQueryable<T> query, Func<T, TKey> keySelector, Func<T, TElement> elementSelector, CancellationToken cancellationToken = default)
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return Task.FromResult(query.ToDictionary(keySelector, elementSelector));

            return IAsyncQueryExecutioner.Executioner.ToDictionaryAsync(query, keySelector, elementSelector);
        }

        public static IQueryable<T> AsNoTracking<T>(this IQueryable<T> query) where T : class
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                return query;

            return IAsyncQueryExecutioner.Executioner.AsNoTracking(query);
        }

        public static IIncludableQueryable<T, TProperty> Include<T, TProperty>(this IQueryable<T> query, Expression<Func<T, TProperty>> navigationPropertyPath)
          where T : class
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                throw new InvalidOperationException("IAsyncQueryExecutioner not found");

            return IAsyncQueryExecutioner.Executioner.Include(query, navigationPropertyPath);
        }

        public static IIncludableQueryable<T, TProperty> ThenInclude<T, TPreviusProperty, TProperty>(this IIncludableQueryable<T, IEnumerable<TPreviusProperty>> query, Expression<Func<TPreviusProperty, TProperty>> navigationPropertyPath)
        where T : class
        {
            if (IAsyncQueryExecutioner.Executioner == null)
                throw new InvalidOperationException("IAsyncQueryExecutioner not found");

            return IAsyncQueryExecutioner.Executioner.ThenInclude(query, navigationPropertyPath);
        }

    }
}
