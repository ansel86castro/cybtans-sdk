using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Testing.Integration
{
    public class ContainerConfig
    {
        public string Image { get; set; }

        public string NamePrefix { get; set; }

        public int ContainerPort { get; set; }

        public int HostPort { get; set; }

        public List<string> Environment { get; set; } = new List<string>();

        public Func<ContainerInfo, Task> WaitFunction { get; set; } 
    }

    public class SqlServerContainer: ContainerConfig
    {
        private readonly string _password;

        public SqlServerContainer(string prefix= "SqlServerIntegrationTest-", int port = 1433, string password = "fH9X3DWt9MwgAnqZ")
        {
            Image = "mcr.microsoft.com/mssql/server:2019-latest";
            HostPort = port;
            ContainerPort = 1433;
            NamePrefix = prefix;
            Environment = new List<string>
                    {
                        "ACCEPT_EULA=Y",
                        $"SA_PASSWORD={password}"
                    };
            _password  = password;
            WaitFunction = c => WaitUntilDatabaseAvailableAsync(c.Port);
        }

        public string Password => _password;

        public static SqlConnection OpenConnection(string port, string password)
        {
            var sqlConnectionString = $"Data Source=localhost,{port};Integrated Security=False;User ID=SA;Password={password}";
            var conn = new SqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }

        private async Task WaitUntilDatabaseAvailableAsync(int databasePort)
        {
            var end = DateTime.UtcNow.AddSeconds(60);
            var connectionEstablised = false;
            while (!connectionEstablised && end > DateTime.UtcNow)
            {
                try
                {
                    var sqlConnectionString = $"Data Source=localhost,{databasePort};Integrated Security=False;User ID=SA;Password={_password}";
                    using var sqlConnection = new SqlConnection(sqlConnectionString);
                    await sqlConnection.OpenAsync();
                    connectionEstablised = true;
                }
                catch
                {
                    // If opening the SQL connection fails, SQL Server is not ready yet
                    await Task.Delay(500);
                }
            }

            if (!connectionEstablised)
            {
                throw new Exception("Connection to the SQL docker database could not be established within 60 seconds.");
            }

            return;
        }
    }

    public class MongoDbContainer: ContainerConfig
    {
        public string Password { get; }

        public MongoDbContainer(string prefix = "MongoDbIntegrationTest-", int port = 27017, string password = "Pass123.")
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
                var client = new MongoClient(ConnectionString);
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
                        // If opening the SQL connection fails, SQL Server is not ready yet
                        await Task.Delay(500);
                    }
                }

                if (!connectionEstablised)
                {
                    throw new Exception("Connection to the MongoDB container could not be established within 60 seconds.");
                }

                return;
            };

        }

        public string ConnectionString => $"mongodb://root:{Password}@localhost:{HostPort}";
    }
}
