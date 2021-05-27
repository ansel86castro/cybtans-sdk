using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Cybtans.Services.Caching
{
    public class RedisOptions
    {
        public string Connection { get; set; }
    }

    public class RedisConnectionProvider
    {
        private readonly RedisOptions _config;
        IConnectionMultiplexer _connection;

        public RedisConnectionProvider(IOptions<RedisOptions> config)
        {
            _config = config.Value;
        }

        public IConnectionMultiplexer GetConnection()
        {
            return _connection ?? (_connection = ConnectionMultiplexer.Connect(_config.Connection));
        }
    }
}
