using Cybtans.Entities.MongoDb.Tests.Models;

namespace Cybtans.Entities.MongoDb.Tests
{
    public class TestMongoDbProvider : MongoClientProvider
    {
        public TestMongoDbProvider(MongoOptions options) : base(options)
        {
            Map<Customer>().ToCollection("customers")
                .Index(x=>x.State, IndexSorting.Ascending)
                .Index(b => b.Combine(b.Descending(x => x.Scoring), b.Ascending(x => x.Name)))
                ;
            
        }
    }
}
