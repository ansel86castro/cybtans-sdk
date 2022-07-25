using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Cybtans.Entities.MongoDb
{
    public class MapConfigBuilder
    {
        internal string _collection;

        internal MapConfigBuilder() { }             

        public virtual Task Initialize(IMongoClientProvider provider) { return Task.CompletedTask; }
    }

    public enum IndexSorting
    {
        Ascending,
        Descending
    }

    public sealed class MapConfigBuilder<T> : MapConfigBuilder
    {
        List<IndexKeysDefinition<T>> indexesKeys = new List<IndexKeysDefinition<T>>();

        public MapConfigBuilder<T> ToCollection(string collection)
        {
            _collection = collection;
            return this;
        }

        public MapConfigBuilder<T> Index(Expression<Func<T, object>> prop, IndexSorting sorting)
        {
            indexesKeys.Add(sorting == IndexSorting.Ascending ?
                Builders<T>.IndexKeys.Ascending(prop) :
                Builders<T>.IndexKeys.Descending(prop));

            return this;
        }

        public MapConfigBuilder<T> Index(Func<IndexKeysDefinitionBuilder<T>, IndexKeysDefinition<T>> func)
        {
            indexesKeys.Add(func(Builders<T>.IndexKeys));
            return this;
        }

        public override async Task Initialize(IMongoClientProvider provider)
        {
            if (indexesKeys.Any())
            {
                var collection = provider.GetCollection<T>();
                await collection.Indexes.CreateManyAsync(indexesKeys.Select(key => new CreateIndexModel<T>(key)));
            }
        }
    }

    public class MongoClientProvider : IMongoClientProvider
    {
        private readonly IMongoClient _client;
        private IMongoDatabase _database;
        private Dictionary<Type, MapConfigBuilder> _mappings = new Dictionary<Type, MapConfigBuilder>();
        private readonly MongoOptions _options;

        public MongoClientProvider(MongoOptions options)
        {
            _client = new MongoClient(options.ConnectionString);
            _options  = options;
        }

        protected MapConfigBuilder<T> Map<T>()
        {
            if(!_mappings.TryGetValue(typeof(T), out var builder))
            {
                builder = new MapConfigBuilder<T>();
                _mappings[typeof(T)] = builder;
            }
            return (MapConfigBuilder<T>)builder;
        }

        public IMongoClient Client => _client;

        public IMongoDatabase Database => _database ?? (_database = _client.GetDatabase(_options.Database));

        public string GetCollectionFor<T>()
        {
            var type = typeof(T);
            var attr = type.GetCustomAttribute<MongoCollectionAttribute>();
            if (attr != null)
            {
               return attr.Name;
            }

            if(_mappings.TryGetValue(type, out var config))
            {
                return config._collection;
            }

            return type.Name;
        }
       
        public IMongoCollection<T> GetCollection<T>()
        {
           return  Database.GetCollection<T>(GetCollectionFor<T>());
        }

        public async Task Initialize()
        {
            foreach (var item in _mappings)
            {
               await item.Value.Initialize(this);
            }
        }
    }
}
