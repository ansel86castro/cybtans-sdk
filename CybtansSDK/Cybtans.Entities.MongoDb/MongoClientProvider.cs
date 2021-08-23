using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Cybtans.Entities.MongoDb
{
    public class MapConfigBuilder
    {
        internal string _collection;
        internal MapConfigBuilder() { }        

        public MapConfigBuilder ToCollection(string collection)
        {
            _collection = collection;
            return this;
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

        protected MapConfigBuilder Map<T>()
        {
            if(!_mappings.TryGetValue(typeof(T), out var builder))
            {
                builder = new MapConfigBuilder();
                _mappings[typeof(T)] = builder;
            }
            return builder;
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


    }
}
