using Cybtans.Services.Caching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Services.Locking
{
    public class LockOptions
    {
        public int ExpireTimeSeconds { get; set; } = (int)TimeSpan.FromMinutes(3).TotalSeconds;

        public int MonitoringTimeMiliseconds { get; set; } = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
    }

    public class DistributedLockProvider : ILockProvider, IDisposable
    {        
        enum LockStatus
        {
            Pending,
            Acquired,            
            Expired,
            Disposed
        }

        readonly IConnectionMultiplexer _connection;
        readonly IDatabase _db;
        private readonly TimeSpan _expire;
        private readonly string _script;
        private readonly List<RedisLock> _locks = new List<RedisLock>();
        private readonly LockOptions _options;
        private readonly ILogger _logger;
        Random _rand = new Random();
        Thread? _lockMonitorThread;
        volatile bool _monitorRunning;
        bool _disposed;

        public DistributedLockProvider(RedisConnectionProvider connectionProvider, IOptions<LockOptions> options, ILogger<DistributedLockProvider> logger)
        {
            _options = options.Value;
            _connection = connectionProvider.GetConnection();
            _db = _connection.GetDatabase();
            _expire = TimeSpan.FromSeconds(_options.ExpireTimeSeconds);

             _script = $@"
local i
i = redis.call('incr',KEYS[1])
if tonumber(i) == 1 then 
redis.call('expire', KEYS[1], {_options.ExpireTimeSeconds}) 
end
return i 
";            
            _logger = logger;            
        }

        public async ValueTask<ILock> GetLock(string key)
        {
            if (_disposed) throw new LockException("LockProvider is disposed");

            StartMonitoring();
            
            var @lock = new RedisLock(key, this);

            //await _db.sScriptEvaluateAsync(_script, new RedisKey[] { new RedisKey(key) });            
            var adquired = await _db.LockTakeAsync(key, @lock.Token, _expire);
            if (adquired)
            {
                _logger.LogInformation("Lock on {Key} adquired", @lock.Key);
                @lock.AdquireLock();
                lock (_locks)
                {
                    _locks.Add(@lock);
                }
                return @lock;
            }

            lock (_locks)
            {
                _locks.Add(@lock);
            }

            _logger.LogInformation("Lock on {Key} pending", @lock.Key);
            return await @lock.CreateTask();         
        }


        private string GetToken()
        {
            lock (_rand)
            {
                byte[] bytes = new byte[32];
                _rand.NextBytes(bytes);

                StringBuilder sb = new StringBuilder(Environment.MachineName);
                sb.Append(":");

                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();               
            }
        }

        private void StartMonitoring()
        {
            if (_lockMonitorThread == null)
            {
                _lockMonitorThread = new Thread(new ThreadStart(OnMonitoring))
                {
                    IsBackground = true
                };
                _monitorRunning = true;
                _lockMonitorThread.Start();
            }                        
        }

        private void StopMonitoring()
        {
            if (_lockMonitorThread != null)
            {
                _monitorRunning = false;
                
                if (_lockMonitorThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                {
                    _lockMonitorThread.Interrupt();
                }
                _lockMonitorThread.Join();
            }
        }

        private void OnMonitoring()
        {
            while (_monitorRunning)
            {
                RedisLock[] locks;
                lock (_locks)
                {
                    locks = _locks.ToArray();
                }

                try
                {
                    foreach (var l in locks)
                    {
                        if (l.Status == LockStatus.Acquired)
                        {
                            if (l.ElapseTimeSinceAdquired > _expire.Subtract(TimeSpan.FromMilliseconds(200)))
                            {
                                //Lock is about to expire so need to extend the lock
                                try
                                {
                                    Extend(l);
                                }
                                catch (LockException)
                                {
                                    l.ExpiredLock();
                                    lock (_locks)
                                    {
                                        _locks.Remove(l);
                                    }
                                }
                            }
                        }
                        else if (l.Status == LockStatus.Pending)
                        {
                            //Try to adquire the lock
                            var adquired = _db.LockTake(l.Key, l.Token, _expire);
                            if (adquired)
                            {
                                _logger.LogInformation("Adquired Lock on {Key}", l.Key);
                                l.AdquireLock();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }

                Thread.Sleep(_options.MonitoringTimeMiliseconds);
            }
        }
       
        private void Remove(RedisLock @lock)
        {
            if (!_db.LockRelease(@lock.Key, @lock.Token))
            {
                _logger.LogInformation("Lock on {Key} expired or the process doesn't own the lock", @lock.Key);
            }

            lock (_locks) 
            {
                _locks.Remove(@lock);
            }
        }

        private async Task ExtendAsync(RedisLock @lock)
        {
            var result = await _db.LockExtendAsync(@lock.Key, @lock.Token, _expire);
            if (!result)
            {
                _logger.LogInformation("Log on {Key} expired or the process doesn't own the lock", @lock.Key);

                lock (_locks)
                {
                    _locks.Remove(@lock);
                }

                throw new LockException($"Lock {@lock.Key} Expired or the process doesn't own the lock");
            }
            @lock.AdquireLock();
        }

        private void Extend(RedisLock @lock)
        {
            var result = _db.LockExtend(@lock.Key, @lock.Token, _expire);
            if (!result)
            {
                _logger.LogInformation("Log on {Key} expired or the process doesn't own the lock", @lock.Key);

                lock (_locks)
                {
                    _locks.Remove(@lock);
                }

                throw new LockException($"Lock {@lock.Key} Expired or the process doesn't own the lock");
            }
            @lock.AdquireLock();
        }

        public void Dispose()
        {
            if (_disposed) return;

            StopMonitoring();

            foreach (var item in _locks.ToArray())
            {
                if (item.Status == LockStatus.Acquired)
                {
                    Remove(item);
                }
            }
            _disposed = true;
        }

        class RedisLock : ILock
        {
            TaskCompletionSource<ILock>? _tcs;
            Stopwatch _stopwatch;
            DistributedLockProvider _provider;
            string _token;

            public RedisLock(string key, DistributedLockProvider provider)
            {
                Key = key;
                _provider = provider;
                _stopwatch = new Stopwatch();
                Status = LockStatus.Pending;
                _token = provider.GetToken();
            }

            public LockStatus Status { get; private set; }

            public TimeSpan ElapseTimeSinceAdquired => _stopwatch.Elapsed;

            public string Token => _token;

            public string Key { get; }

            public void AdquireLock()
            {                
                if (_stopwatch.IsRunning)
                    _stopwatch.Stop();

                _stopwatch.Start();

                var prevStatus = Status;
                Status = LockStatus.Acquired;
                if (prevStatus == LockStatus.Pending && _tcs != null)
                {
                    _tcs.SetResult(this);
                }                
            }

            public void ExpiredLock()
            {
                _stopwatch.Stop();
                Status = LockStatus.Expired;

            }

            internal Task<ILock> CreateTask()
            {
                _tcs = new TaskCompletionSource<ILock>();
                return _tcs.Task;
            }

            public void Dispose()
            {
                if (Status == LockStatus.Disposed)
                    throw new LockException("The lock has been disposed");

                if (Status == LockStatus.Pending)
                {
                    if (_tcs != null)
                    {
                        _tcs.SetCanceled();
                    }
                }
                else
                {
                    _provider.Remove(this);
                }
                _stopwatch.Stop();

                Status = LockStatus.Disposed;
            }

            public Task ExtendLock()
            {
                if (Status != LockStatus.Acquired)
                    throw new InvalidOperationException($"Lock {Key} not adquired");

                return _provider.ExtendAsync(this);
            }

            public override string ToString()
            {
                return $"{Key}, Status:{Status}";
            }
        }

      
    }
}
