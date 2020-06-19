using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Services.Utils
{
    public class WorkQueue : IDisposable
    {
        struct WorkItem
        {
            public Delegate Action;
            public CancellationToken CancellationToken;
            public AutoResetEvent WaitHandler;
            public object Args;
        }

        Queue<WorkItem> queue;
        Thread loopThread;
        volatile bool running;
        volatile bool waiting;
        AutoResetEvent waitHandle;

        private object lockItem = new object();

        public WorkQueue()
        {
            queue = new Queue<WorkItem>();
            waitHandle = new AutoResetEvent(false);

            running = true;
            loopThread = new Thread(LoopMain); 
            loopThread.Start();
        }

        public Task Enqueue<T>(Func<T, Task> workFunc, T arg, CancellationToken cancellationToken = default)
        {
            var workItem = new WorkItem
            {
                Action = workFunc,
                CancellationToken = cancellationToken,
                WaitHandler = new AutoResetEvent(false),
                Args = arg
            };
            lock (lockItem)
            {
                queue.Enqueue(workItem);
                if (waiting)
                    waitHandle.Set();
            }

            return WaitOneAsync(workItem.WaitHandler);

        }

        private async void LoopMain()
        {
            while (running)
            {
                WorkItem workItem = new WorkItem();
                lock (lockItem)
                {
                    if (queue.Count > 0)
                    {
                        workItem = queue.Dequeue();
                    }
                    else
                    {
                        waiting = true;
                    }
                }

                if (waiting)
                {
                    waitHandle.WaitOne();
                    waiting = false;
                }

                else if (workItem.Action != null && !workItem.CancellationToken.IsCancellationRequested)
                {
                    await (Task)workItem.Action.DynamicInvoke(workItem.Args);
                    workItem.WaitHandler.Set();
                }

            }
        }

        public void Dispose()
        {
            if (running)
            {
                running = false;
                if (loopThread != null)
                {
                    lock (lockItem)
                    {
                        if (waiting)
                            waitHandle.Set();
                    }
                    loopThread.Join();
                }
            }
        }

        public static Task WaitOneAsync(WaitHandle waitHandle)
        {
            if (waitHandle == null) throw new ArgumentNullException("waitHandle");

            var tcs = new TaskCompletionSource<bool>();
            var rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle,
                delegate { tcs.TrySetResult(true); }, null, -1, true);
            var t = tcs.Task;
            t.ContinueWith(_ => rwh.Unregister(null));
            return t;
        }


    }
}
