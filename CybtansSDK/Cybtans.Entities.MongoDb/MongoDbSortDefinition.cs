using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Cybtans.Entities.MongoDb
{
    public class MongoDbSortDefinition<T> : IObjectSortDefinition<T> where T : class
    {
        public MongoDbSortDefinition(SortDefinition<T> sort)
        {
            Sort = sort;
        }

        public SortDefinition<T> Sort { get; }
    }

    public class MongoDbSortDefinitionBuilder<T> : ISortDefinitionBuilder<T> where T : class
    {
        public IObjectSortDefinition<T> Ascending(Expression<Func<T, object>> prop)
        {            
            return new MongoDbSortDefinition<T>(Builders<T>.Sort.Ascending(prop));
        }

        public IObjectSortDefinition<T> Descending(Expression<Func<T, object>> prop)
        {
            return new MongoDbSortDefinition<T>(Builders<T>.Sort.Descending(prop));
        }

        public IObjectSortDefinition<T> Combine(params IObjectSortDefinition<T>[] sorts)
        {
            return new MongoDbSortDefinition<T>(Builders<T>.Sort.Combine(sorts.Cast<MongoDbSortDefinition<T>>().Select(x=> x.Sort)));
        }

        public IObjectSortDefinition<T> Combine(IEnumerable<IObjectSortDefinition<T>> sorts)
        {
            return new MongoDbSortDefinition<T>(Builders<T>.Sort.Combine(sorts.Cast<MongoDbSortDefinition<T>>().Select(x => x.Sort)));
        }
    }
}
