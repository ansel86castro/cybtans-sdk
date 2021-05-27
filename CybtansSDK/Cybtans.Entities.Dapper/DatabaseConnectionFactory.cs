using Dapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cybtans.Entities.Dapper
{

    public class DatabaseConectionFactoryOptions
    {
        public string ConnectionString { get; set; }

        public Func<IDbConnection> ConnectionFactoryDelegate { get; set; }
    }

    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly DatabaseConectionFactoryOptions _options;

        public DatabaseConnectionFactory(DatabaseConectionFactoryOptions options)
        {
            _options = options;
        }


        public virtual IDatabaseConnection GetConnection() => new DatabaseConnection(GetDbConnection());

        protected virtual IDbConnection GetDbConnection()
        {
            if (_options.ConnectionFactoryDelegate != null)
                return _options.ConnectionFactoryDelegate() ?? throw new InvalidOperationException("Connection can not be null");

            if (string.IsNullOrEmpty(_options.ConnectionString))
                throw new InvalidOperationException("ConnectionString was not found");

            return new SqlConnection(_options.ConnectionString);
        }

        private sealed class DatabaseConnection : IDatabaseConnection
        {
            private static readonly ConcurrentDictionary<string, string> _upsertCache = new();

            private readonly IDbConnection _connection;

            public DatabaseConnection(IDbConnection connection)
            {
                _connection = connection;
            }

            public void Dispose()
            {
                _connection.Dispose();
            }

            public Task<int> ExecuteAsync(string sql, object args = null, IDbTransaction dbTransaction = null)
            {
                return _connection.ExecuteAsync(sql, args, dbTransaction);
            }

            public Task<T> ExecuteScalarAsync<T>(string sql, object args = null, IDbTransaction dbTransaction = null)
            {
                return _connection.ExecuteScalarAsync<T>(sql, args, dbTransaction);
            }

            public Task<IEnumerable<T>> QueryAsync<T>(string sql, object args = null, IDbTransaction dbTransaction = null)
            {
                return _connection.QueryAsync<T>(sql, args, dbTransaction);
            }

            public Task<IEnumerable<TFirst>> QueryAsync<TFirst, TSecond, TThird>(string sql, Func<TFirst, TSecond, TThird, TFirst> map, object param = null, string splitOn = "Id", IDbTransaction transaction = null)
            {
                return _connection.QueryAsync(sql, map, param, transaction, true, splitOn);
            }

            public Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string sql, Type[] types, Func<object[], TReturn> map, object param = null, string splitOn = "Id", IDbTransaction transaction = null)
            {
                return _connection.QueryAsync(sql, types, map, param, transaction, true, splitOn);
            }

            public Task<T> QueryFirstAsync<T>(string sql, object args = null, IDbTransaction dbTransaction = null)
            {
                return _connection.QueryFirstAsync<T>(sql, args, dbTransaction);
            }

            public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object args = null, IDbTransaction dbTransaction = null)
            {
                return _connection.QueryFirstOrDefaultAsync<T>(sql, args, dbTransaction);
            }

            public Task<T> Upsert<T>(string[] keys, string table, bool insertKeys, object args, Dictionary<string, string> onUpdateArgs = null, Dictionary<string, string> onInsertArgs = null)
            {
                string sql = BuildUpsertQuery(keys, table, insertKeys, args);

                return _connection.QueryFirstAsync<T>(sql, args);
            }

            public Task<T> Upsert<T>(string key, string table, object args, Dictionary<string, string> onUpdateArgs = null, Dictionary<string, string> onInsertArgs = null) =>
                Upsert<T>(new[] { key }, table, false, args, onUpdateArgs, onInsertArgs);

            private string BuildUpsertQuery(string[] keys, string table, bool insertKeys, object args, Dictionary<string, string> onUpdateArgs = null, Dictionary<string, string> onInsertArgs = null)
            {
                if (_connection is not SqlConnection)
                    throw new InvalidOperationException("Merge is not supported for the current connection");

                var type = args.GetType();
                return _upsertCache.GetOrAdd($"{table}:{type.FullName}", _ =>
                {
                    var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(x => x.Name).ToList();

                    var argValues = string.Join(", ", props.Select(x => "@" + x));
                    var condition = keys.Select(k => $"Target.[{k}] = Source.[{k}]").Aggregate((x, y) => $"{x} AND {y}");

                    var columns = (insertKeys ? props : props.Except(keys)).Select(x => $"[{x}]").ToList();

                    var update = string.Join(", ", columns.Select(c => $"{c} = Source.{c}"));
                    var sourceColumns = string.Join(", ", columns.Select(c => $"Source.{c}"));

                    var onUpdate = onUpdateArgs?.Any() ?? false ? "," + string.Join(", ", onUpdateArgs.Select(x => $"[{x.Key}] = {x.Value}")) : "";
                    var onInsertColumns = onInsertArgs?.Any() ?? false ? "," + string.Join(", ", onInsertArgs.Select(x => $"[{x.Key}]")) : "";
                    var onInsertValues = onInsertArgs?.Any() ?? false ? "," + string.Join(", ", onInsertArgs.Select(x => $"{x.Value}")) : "";

                    var columnString = string.Join(",", columns);

                    var sql = $@"
                MERGE {table} AS Target USING ( VALUES ({argValues})) 
                AS Source ({string.Join(",", props.Select(x => $"[{x}]"))})
                ON {condition}
                WHEN MATCHED THEN 
                UPDATE SET {update}{onUpdate}
                WHEN NOT MATCHED BY TARGET THEN 
                INSERT ({columnString}{onInsertColumns}) VALUES ( {sourceColumns}{onInsertValues})
                OUTPUT INSERTED.*;";

                    return sql;
                });
            }
        }


    }

}
