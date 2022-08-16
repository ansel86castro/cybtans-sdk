namespace Cybtans.Entities.MongoDb
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public bool TlsEnable { get; set; }
    }
}
