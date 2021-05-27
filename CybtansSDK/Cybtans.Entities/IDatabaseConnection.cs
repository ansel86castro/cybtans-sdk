using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities
{
    public interface IDatabaseConnection : IDisposable
    {
        Task<int> ExecuteAsync(string sql, object? args = null, IDbTransaction? dbTransaction = null);

        Task<T> ExecuteScalarAsync<T>(string sql, object? args = null, IDbTransaction? dbTransaction = null);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? args = null, IDbTransaction? dbTransaction = null);

        Task<T> QueryFirstAsync<T>(string sql, object? args = null, IDbTransaction? dbTransaction = null);

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? args = null, IDbTransaction? dbTransaction = null);

        Task<IEnumerable<TFirst>> QueryAsync<TFirst, TSecond, TThird>(string sql, Func<TFirst, TSecond, TThird, TFirst> map, object? param = null, string splitOn = "Id", IDbTransaction? transaction = null);

        Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string sql, Type[] types, Func<object[], TReturn> map, object? param = null, string splitOn = "Id", IDbTransaction? transaction = null);

        Task<T> Upsert<T>(string[] keys, string table, bool insertKeys, object args, Dictionary<string, string>? onUpdateArgs = null, Dictionary<string, string>? onInsertArgs = null);

        Task<T> Upsert<T>(string key, string table, object args, Dictionary<string, string>? onUpdateArgs = null, Dictionary<string, string>? onInsertArgs = null);


    }
}