using Cybtans.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public static class Extensions
    {
        public static void AddSoftDeleteQueryFilters(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                //other automated configurations left out
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }

        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(Extensions)
                .GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(entityData.ClrType);
            var filter = methodToCall.Invoke(null, null);
            entityData.SetQueryFilter((LambdaExpression)filter);
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, ISoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }

        public static async Task<PagedQuery<T>> AsPagedQuery<T>(this IQueryable<T> query, int page, int pageSize,            
            Func<IQueryable<T>, IOrderedQueryable<T>> sort)
        {
            var count = await query.LongCountAsync();

            page = Math.Max(1, page);
            pageSize = Math.Min(pageSize, 100);
            if (pageSize <= 0) pageSize = 100;

            var skip = pageSize * (page - 1);
            var totalPages = count / pageSize + (count % pageSize == 0 ? 0 : 1);

            if (sort != null)
                query = sort(query);

            var items = query.Skip(skip).Take(pageSize);
            return new PagedQuery<T>(items, page, totalPages, count);
        }
    }
}
