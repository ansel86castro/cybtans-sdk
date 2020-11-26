using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Services.Locking
{
    public class MemoryLockProvider : ILockProvider
    {
        private readonly Dictionary<string, MemoryLock> _locks = new Dictionary<string, MemoryLock>();

        public async ValueTask<ILock> GetLock(string key)
        {
            MemoryLock l;

            lock (_locks)
            {
                if(!_locks.TryGetValue(key, out l))
                {
                    l = new MemoryLock(key, this);
                    _locks.Add(key, l);
                    return l;
                }
            }            

           return await l.CreateTask();
        }

        private void Remove(string  key)
        {
            lock (_locks)
            {
                if (!_locks.Remove(key, out _))
                {
                    throw new InvalidOperationException($"The Lock for {key} was already disposed");
                }
            }
        }

        private class MemoryLock : ILock
        {
            private readonly string _key;
            MemoryLockProvider _provider;            
            Queue<TaskCompletionSource<ILock>> _tcs = new Queue<TaskCompletionSource<ILock>>();
            
            public MemoryLock(string key, MemoryLockProvider provider)
            {               
                _key = key;
                _provider = provider;               
            }
                     

            public void Dispose()
            {
                lock (_tcs)
                {
                    if(_tcs.TryDequeue(out var l))
                    {
                        l.SetResult(this);
                    }
                    else
                    {
                        _provider.Remove(_key);
                    }
                }                
            }

            public Task ExtendLock()
            {
               return Task.CompletedTask;
            }

            internal Task<ILock> CreateTask()
            {
                lock (_tcs)
                {
                    var tcs = new TaskCompletionSource<ILock>();
                    _tcs.Enqueue(tcs);

                    return tcs.Task;
                }
            }
        }
    }
}
