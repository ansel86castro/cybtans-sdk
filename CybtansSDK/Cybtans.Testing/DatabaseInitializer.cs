using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Testing
{
    public class DatabaseInitOptions
    {
        public bool CreateDatabase { get; set; }

        public string ConnectionName { get; set; }

        public List<string> InitializationScriptPaths { get; set; } = new List<string>();

    }

    public class DatabaseInitializer
    {
        public DatabaseInitializer() : this(BaseFixture.CreateConfiguration()) { }

        public DatabaseInitializer(IConfiguration configuration, ILogger<DatabaseInitializer> logger = null)
        {
            Configuration = configuration;
            SeedOptions = new DatabaseInitOptions();
            configuration.Bind("DatabaseInitOptions", SeedOptions);

            if (logger == null)
            {
                var loggerFactory = new LoggerFactory();
                loggerFactory.AddProvider(new TraceLoggerProvider());
                logger = loggerFactory.CreateLogger<DatabaseInitializer>();
            }

            Logger = logger;
        }

        public static bool ContainsDatabaseOptions(IConfiguration configuration)
        {
            return configuration.GetSection("DatabaseInitOptions").Exists();
        }

        public IConfiguration Configuration { get; }

        public ILogger<DatabaseInitializer> Logger { get; }

        public DatabaseInitOptions SeedOptions { get; }

        public bool ContainsSeedOptions => SeedOptions != null && SeedOptions.InitializationScriptPaths.Any();


        public async Task<bool> InitializeAsync(bool forceCreate = false)
        {
            if (!ContainsSeedOptions)
            {
                return false;
            }

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(Configuration.GetConnectionString(SeedOptions.ConnectionName));
            var database = sb.InitialCatalog;
            sb.InitialCatalog = "master";

            using var conn = new SqlConnection(sb.ToString());

            await conn.OpenAsync();

            if (SeedOptions.CreateDatabase || forceCreate)
            {
                await CreateDatabase(conn, database, Logger);
            }

            await RunInitializationScripts(conn, SeedOptions.InitializationScriptPaths, Logger);
            return true;
        }

        private static async Task CreateDatabase(SqlConnection conn, string database, ILogger logger)
        {
            logger.LogDebug($"Database {database} Creation Started");

            var createDbScript =
             @$"                  
                    IF DB_ID('{database}') IS NOT NULL
                    BEGIN
                        DROP DATABASE {database}    
                    END

                    CREATE DATABASE {database}
                  ";

            logger.LogDebug($"Executing script: { createDbScript}");

            using SqlCommand createDatabaseCmd = new SqlCommand(createDbScript, conn);
            try
            {
                var rows = await createDatabaseCmd.ExecuteNonQueryAsync();
                logger.LogDebug($"{rows} Affected");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed Creation of Database {database}");
                throw;
            }

            logger.LogDebug($"Database {database} Creation Completed");

            await conn.ChangeDatabaseAsync(database);

            logger.LogDebug($"Changed Database to {database}");
        }

        private static async Task RunInitializationScripts(SqlConnection connection, List<string> filenames, ILogger logger)
        {
            logger.LogDebug($"Initializing Database {connection.Database}");

            foreach (var filepath in filenames)
            {
                logger.LogDebug($"Running Script {filepath}");

                var sql = File.ReadAllText(filepath);
                using SqlCommand cmd = new SqlCommand(sql, connection);
                try
                {
                    var rows = await cmd.ExecuteNonQueryAsync();
                    logger.LogDebug($"{rows} Affected");
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Failed Script {filepath}");
                    throw;
                }
            }

            logger.LogDebug($"Initialization of Database {connection.Database} Completed");
        }
    }
}
