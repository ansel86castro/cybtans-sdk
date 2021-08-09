using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cybtans.Entities
{
    public class PagedQuery<T>
    {
        public PagedQuery(IQueryable<T> query, long page, long totalPages, long totalCount)
        {
            Query = query;
            Page = page;
            TotalPages = totalPages;
            TotalCount = totalCount;
        }

        public IQueryable<T> Query { get; }

        public long Page { get; }

        public long TotalPages { get; }

        public long TotalCount { get; }
    }

    public class PagedEnumerable<T>
    {
        public PagedEnumerable(IEnumerable<T> items, long page, long totalPages, long totalCount)
        {
            Items = items;
            Page = page;
            TotalPages = totalPages;
            TotalCount = totalCount;
        }

        public IEnumerable<T> Items { get; }

        public long Page { get; }

        public long TotalPages { get; }

        public long TotalCount { get; }
    }
}
