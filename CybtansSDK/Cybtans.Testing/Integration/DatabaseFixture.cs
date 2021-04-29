using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Testing.Integration
{

    public class DatabaseFixture : IAsyncLifetime
    {
        public class DatabaseInitOptions
        {
            public bool CreateDatabase { get; set; }

            public string ConnectionName { get; set; }

            public List<string> InitializationScriptPaths { get; set; } = new List<string>();

        }

        private string _containerId;
        private string _port;
        private IConfigurationRoot _config;
        private string _database;

        public IConfiguration Configuration => _config;

        public async Task InitializeAsync()
        {
            (_containerId, _port) = await DockerSqlDatabaseUtilities.StartSqlServerContainer();

            _config = CreateConfiguration();
            var seedOptions = new DatabaseInitOptions();
            _config.Bind(nameof(DatabaseInitOptions), seedOptions);

            var connectionString = _config.GetConnectionString(seedOptions.ConnectionName);
            if (connectionString == null)
                throw new InvalidOperationException("Connection String not found");

            var sb = new SqlConnectionStringBuilder(connectionString);
            _database = sb.InitialCatalog;

            using (var conn = DockerSqlDatabaseUtilities.OpenConnection(_port))
            {
                await CreateDatabase(conn, _database);
                await RunInitializationScripts(conn, seedOptions.InitializationScriptPaths);
            }

            await OnInitializedAsync();

        }

        protected virtual Task OnInitializedAsync() { return Task.CompletedTask; }

        public async Task DisposeAsync()
        {
            if (!string.IsNullOrEmpty(_containerId))
            {
                await DockerSqlDatabaseUtilities.RemoveContainer(_containerId);
            }
        }

        protected virtual Task OnDisposeAsync()
        {
            return Task.CompletedTask;
        }

        public static IConfigurationRoot CreateConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.Test.json", optional: true)
              .AddEnvironmentVariables();

            return configBuilder.Build();
        }

        public SqlConnection OpenDbConnection()
        {
            var sqlConnectionString = $"Data Source=localhost,{_port};Database={_database};Integrated Security=False;User ID=SA;Password={DockerSqlDatabaseUtilities.SQLSERVER_SA_PASSWORD}";
            return new SqlConnection(sqlConnectionString);
        }
       
        private static async Task CreateDatabase(SqlConnection conn, string database)
        {
            Trace.TraceInformation($"Database {database} Creation Started");

            var createDbScript =
             @$"                  
                    IF DB_ID('{database}') IS NOT NULL
                    BEGIN
                        DROP DATABASE {database}    
                    END

                    CREATE DATABASE {database}
                  ";

            Trace.TraceInformation($"Executing script: { createDbScript}");

            using SqlCommand createDatabaseCmd = new SqlCommand(createDbScript, conn);
            try
            {
                var rows = await createDatabaseCmd.ExecuteNonQueryAsync();
                Trace.TraceInformation($"{rows} Affected");
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failed Creation of Database {database}, {e.Message}");
                throw;
            }

            Trace.TraceInformation($"Database {database} Creation Completed");

            await conn.ChangeDatabaseAsync(database);

            Trace.TraceInformation($"Changed Database to {database}");
        }

        private static async Task RunInitializationScripts(SqlConnection connection, List<string> filenames)
        {
            Trace.TraceInformation($"Initializing Database {connection.Database}");

            foreach (var filepath in filenames)
            {
                Trace.TraceInformation($"Running Script {filepath}");

                var sql = File.ReadAllText(filepath);
                using SqlCommand cmd = new SqlCommand(sql, connection);
                try
                {
                    var rows = await cmd.ExecuteNonQueryAsync();
                    Trace.TraceInformation($"{rows} Affected");
                }
                catch (Exception e)
                {
                    Trace.TraceError($"Failed Script {filepath}, {e.Message}");
                    throw;
                }
            }

            Trace.TraceInformation($"Initialization of Database {connection.Database} Completed");
        }      

    }
}
