using Cybtans.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cybtans.Expressions;
using System.Threading.Tasks;
using Cybtans.Entities.Extensions;

namespace Cybtans.Services.Extensions
{
    public static class QueryExtensor
    {
        public static async Task<PagedQuery<T>> PageBy<T>(this IQueryable<T> query, string filter, string sort, int? skip = 0, int? take = 50)
        {          
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                query = query.OrderBy(sort);
            }

            var count = await query.CountAsync();
            return query.PageBy(count, skip ?? 0, take ?? 100);
        }


    }
}
