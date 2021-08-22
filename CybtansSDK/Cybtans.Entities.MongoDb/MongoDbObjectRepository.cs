using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities.MongoDb
{
    public class MongoDbObjectRepository<T> : IObjectRepository<T>
        where T : class
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDb;
        private IMongoCollection<T> _collection;

        public MongoDbObjectRepository(IMongoClientProvider provider)
        {
            _mongoClient = provider.Client;
            _mongoDb = provider.Database;            
            _collection = provider.GetCollection<T>();            
        }

        public MongoDbObjectRepository(IMongoClient mongoClient, string database, string collection)
        {
            _mongoClient = mongoClient;
            _mongoDb = mongoClient.GetDatabase(database);
            _collection = _mongoDb.GetCollection<T>(collection ?? typeof(T).Name);
        }

        protected IMongoCollection<T> Collection => _collection;
        protected IMongoDatabase Database => _mongoDb;
        protected IMongoClient MongoClient => _mongoClient;

        public virtual async Task<T> AddAsync(T item)
        {
            await _collection.InsertOneAsync(item);
            return item;
        }

        public virtual Task AddRangeAsync(IEnumerable<T> items)
        {
            return _collection.InsertManyAsync(items);
        }
     
        public async Task<PagedList<T>> GetManyAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? sortBy = null)
        {
            var count = await (filter != null ?
                _collection.CountDocumentsAsync(filter) :
                _collection.CountDocumentsAsync(new BsonDocument()));

            var totalPages = count / pageSize + (count % pageSize == 0 ? 0 : 1);

            var query = filter != null ? _collection.Find(filter) : _collection.Find(new BsonDocument());            
            if (sortBy != null)
            {
                query = query.SortBy(sortBy);
            }

            query = query.Skip((page - 1) * pageSize)
              .Limit(pageSize);

            return new (await query.ToListAsync(), page, totalPages, count);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter, ReadConsistency consistency = ReadConsistency.Strong)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<long> UpdateAsync(Expression<Func<T, bool>> filter, IDictionary<string, object> data)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var res = await _collection.UpdateOneAsync(filter, new BsonDocument(data));
            return res.ModifiedCount;
        }

        public async Task<long> UpdateAsync(Expression<Func<T, bool>> filter, T item)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

           var res =  await _collection.ReplaceOneAsync(filter, item);
           return res.ModifiedCount;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var cursor = _collection.Find(new BsonDocument()).ToCursor();
            return new ObjectRepositoryEnumerator(cursor);
        }


        public IAsyncEnumerator<T> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? sortBy = null)
        {
            var query = filter != null ? _collection.Find(filter) : _collection.Find(new BsonDocument());             
            if (sortBy != null)
            {
                query = query.SortBy(sortBy);
            }
            var cursor = query.ToCursor();
            return new ObjectRepositoryEnumerator(cursor);

        }

        public async Task<long> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            var result = await  _collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        public sealed class ObjectRepositoryEnumerator : IAsyncEnumerator<T>
        {
            IAsyncCursor<T> _cursor;
            volatile IEnumerator<T>? _current;

            public ObjectRepositoryEnumerator(IAsyncCursor<T> cursor)
            {
                _cursor = cursor;
                _current = null;
            }

            public T Current
            {
                get
                {
                    if (_current == null)
                        throw new InvalidOperationException("You must call move next firt or the collection has no more elemets");
                    
                    return _current.Current;
                }
            }

            public ValueTask DisposeAsync()
            {
                _current?.Dispose();
                _cursor.Dispose();        
                
                return ValueTask.CompletedTask;
            }

            public async ValueTask<bool> MoveNextAsync()
            {
                return _current?.MoveNext() ?? false ? true : await FetchNext();
            }

            private async Task<bool> FetchNext()
            {
                _current?.Dispose();

                if (!await _cursor.MoveNextAsync())
                {
                    return false;
                }               

                _current = _cursor.Current.GetEnumerator();
                return _current == null ? false : _current.MoveNext();
            }
        }
    }

    public class MongoDbObjectRepository<T, TKey> : MongoDbObjectRepository<T>, IObjectRepository<T, TKey>
        where T : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public MongoDbObjectRepository(IMongoClientProvider provider) : base(provider)
        {
        }

        public MongoDbObjectRepository(IMongoClient mongoClient, string database, string collection)
            : base(mongoClient, database, collection)
        {
        }

        public Task<long> DeleteAsync(TKey id) => DeleteAsync(x => x.Id.Equals(id));

        public Task<T> Get(TKey id, ReadConsistency consistency = ReadConsistency.Default) =>
            Get(x => x.Id.Equals(id), consistency);

        public Task<long> UpdateAsync(TKey id, IDictionary<string, object> data) =>
            UpdateAsync(x => x.Id.Equals(id), data);

        public Task<long> UpdateAsync(TKey id, T item) =>
            UpdateAsync(x => x.Id.Equals(id), item);
    }
}
