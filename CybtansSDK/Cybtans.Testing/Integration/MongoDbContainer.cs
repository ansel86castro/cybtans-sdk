using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Testing.Integration
{
    public class MongoDbContainerConfig: ContainerConfig
    {
        public string Password { get; }

        public MongoDbContainerConfig(string prefix = "MongoDbIntegrationTest-", int port = 27017, string password = "Pass123.")
        {
            NamePrefix = prefix;
            Image = "mongo:latest";
            Password = password;
            HostPort = port;
            ContainerPort = 27017;
            Environment = new List<string>
            {
                "MONGO_INITDB_ROOT_USERNAME=root",
                $"MONGO_INITDB_ROOT_PASSWORD={Password}"
            };

            WaitFunction = async c =>
            {
                var end = DateTime.UtcNow.AddSeconds(60);
                var connectionEstablised = false;
                var client = new MongoClient(GetConnectionString(c));
                while (!connectionEstablised && end > DateTime.UtcNow)
                {
                    try
                    {
                        var cursor = await client.ListDatabasesAsync();
                        var databases = await cursor.ToListAsync();
                        if (databases.Any())
                        {
                            connectionEstablised = true;
                        }
                    }
                    catch
                    {                        
                        await Task.Delay(1000);
                    }
                }

                if (!connectionEstablised)
                {
                    throw new Exception("Connection to the MongoDB container could not be established within 60 seconds.");
                }

                return;
            };

        }
        
        public string GetConnectionString(ContainerInfo c)
        {
            if (!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("FROM_CONTAINER")))
            {
                return $"mongodb://root:{Password}@{c.IPAddress}:{c.ContainerPort}";
            }

            return $"mongodb://root:{Password}@localhost:{c.HostPort}";
        }
    }
}
