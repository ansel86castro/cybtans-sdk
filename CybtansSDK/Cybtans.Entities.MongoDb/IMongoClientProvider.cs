using MongoDB.Driver;
using System.Threading.Tasks;

namespace Cybtans.Entities.MongoDb
{
    public interface IMongoClientProvider
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
        IMongoCollection<T> GetCollection<T>();
    }
}