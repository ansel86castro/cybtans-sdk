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
            await _collection.InsertOneAsync(item).ConfigureAwait(false);            
            return item;
        }

        public virtual Task AddRangeAsync(IEnumerable<T> items)
        {
            return _collection.InsertManyAsync(items);
        }
     
        public async Task<PagedList<T>> GetManyAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? sortBy = null, bool descending = false)
        {
            var count = await (filter != null ?
                _collection.CountDocumentsAsync(filter).ConfigureAwait(false) :
                _collection.CountDocumentsAsync(new BsonDocument()).ConfigureAwait(false));

            var totalPages = count / pageSize + (count % pageSize == 0 ? 0 : 1);

            var query = filter != null ? _collection.Find(filter) : _collection.Find(new BsonDocument());
            if (sortBy != null)
            {
                query = descending ? query.SortByDescending(sortBy) : query.SortBy(sortBy);
            }

            query = query.Skip((page - 1) * pageSize)
              .Limit(pageSize);

            return new (await query.ToListAsync().ConfigureAwait(false), page, totalPages, count);
        }

        public Task<T> Get(Expression<Func<T, bool>> filter, ReadConsistency consistency = ReadConsistency.Strong)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return _collection.Find(filter).FirstOrDefaultAsync();
        }

        public Task<T> UpdateAsync(Expression<Func<T, bool>> filter, IDictionary<string, object?> data)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            UpdateDefinition<T>? update = null;

            foreach (var kv in data)
            {
                if (update == null)
                {
                    update = Builders<T>.Update.Set(kv.Key, kv.Value);
                }
                else
                {
                    update = update.Set(kv.Key, kv.Value);
                }
            }

            if (update == null) throw new InvalidOperationException("No data to update");

            return _collection.FindOneAndUpdateAsync(
                filter: filter,
                update: update,
                options: new FindOneAndUpdateOptions<T>
                {
                    ReturnDocument = ReturnDocument.After                   
                }) ?? throw new EntityNotFoundException("Entity not found");            
        }

        public Task<T> UpdateAsync(Expression<Func<T, bool>> filter, T item)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            return _collection.FindOneAndReplaceAsync(filter, item, new FindOneAndReplaceOptions<T>
            {
                ReturnDocument = ReturnDocument.After
            });
        }

        public Task<T> UpdateAsync(Expression<Func<T, bool>> filter, object data)
        {
            return UpdateAsync(filter, Utils.ToDictionary(data));           
        }


        public async Task<long> UpdateManyAsync(Expression<Func<T, bool>> filter, IDictionary<string, object?> data)
        {
           var result = await _collection.UpdateManyAsync(
                 filter: filter,
                 update : new BsonDocument(data)).ConfigureAwait(false);
            return result.ModifiedCount;
        }

        public async Task<long> UpdateManyAsync(Expression<Func<T, bool>> filter, object data)
        {
            var result = await _collection.UpdateManyAsync(
                 filter: filter,
                 update: data.ToBsonDocument()).ConfigureAwait(false);
            return result.ModifiedCount;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var cursor = _collection.Find(new BsonDocument()).ToCursor();
            return new ObjectRepositoryEnumerator(cursor);
        }

        public IAsyncEnumerator<T> EnumerateAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? sortBy = null)
        {
            var query = filter != null ? _collection.Find(filter) : _collection.Find(new BsonDocument());             
            if (sortBy != null)
            {
                query = query.SortBy(sortBy);
            }
            var cursor = query.ToCursor();
            return new ObjectRepositoryEnumerator(cursor);
        }

        public Task<List<T>> ListAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? sortBy = null)
        {
            var query = filter != null ? _collection.Find(filter) : _collection.Find(new BsonDocument());
            if (sortBy != null)
            {
                query = query.SortBy(sortBy);
            }
            return query.ToListAsync();
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

        public async Task DeleteAsync(TKey id) {
            var count = await DeleteAsync(x => x.Id.Equals(id));
            if (count == 0) throw new EntityNotFoundException($"Entity {id} not found");
        }

        public Task<T> Get(TKey id, ReadConsistency consistency = ReadConsistency.Default) =>
            Get(x => x.Id.Equals(id), consistency);

        public Task<T> UpdateAsync(TKey id, IDictionary<string, object?> data) =>
            UpdateAsync(x => x.Id.Equals(id), data);

        public Task<T> UpdateAsync(TKey id, T item) =>
            UpdateAsync(x => x.Id.Equals(id), item);

        public Task<T> UpdateAsync(TKey id, object data) =>
            UpdateAsync(x => x.Id.Equals(id), data);

    }
}
