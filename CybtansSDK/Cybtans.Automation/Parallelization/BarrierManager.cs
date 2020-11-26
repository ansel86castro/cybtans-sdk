using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Automation
{
    public partial class BarrierManager:IDisposable
    {
        public static TimeSpan WaitHandleTimeout = TimeSpan.FromMinutes(5);

        private Dictionary<string, WaitHandleBarrier> _waiHandlers = new Dictionary<string, WaitHandleBarrier>();      

        public static BarrierManager Manager { get; } = new BarrierManager();     

        protected void RemoveHandler(string barrier)
        {
            WaitHandleBarrier? handle;

            lock (_waiHandlers)
            {
                if(_waiHandlers.TryGetValue(barrier , out handle))
                {                    
                    _waiHandlers.Remove(barrier);
                }                
            }

            handle?.Dispose();
        }

        
        public WaitHandleBarrier GetWaitHandler(string barrier)
        {
            CheckDisposed();
          
            lock (_waiHandlers)
            {
                if (!_waiHandlers.TryGetValue(barrier, out var waitHandle))
                {
                    waitHandle = new WaitHandleBarrier(barrier);
                    _waiHandlers.Add(barrier, waitHandle);
                }

                return waitHandle;
            }            
          
        }       

        public async Task Wait(string barrier)
        {
            CheckDisposed();            

           var handler = GetWaitHandler(barrier);
           await handler.CreateTask();
        }

        #region IDisposable Support

        protected bool isDisposed = false; 

        protected void CheckDisposed()
        {
            if (isDisposed) throw new InvalidOperationException("WebDriverManager is Disposed");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    foreach (var item in _waiHandlers)
                    {
                        item.Value.Dispose();
                    }

                    _waiHandlers.Clear();
                    _waiHandlers.TrimExcess(0);
                }
             
                isDisposed = true;
            }
        }
     
     
        public void Dispose()
        {            
            Dispose(true);          
            GC.SuppressFinalize(this);
        }
        
        #endregion
    }

}

#nullable restore
