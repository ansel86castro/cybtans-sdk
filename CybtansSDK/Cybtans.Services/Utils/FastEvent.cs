using System.Threading;

namespace Cybtans.Services.Utils
{
    public class FastEvent : IFastEvent
    {
        protected AutoResetEvent manualResetEvent = new AutoResetEvent(false);

        public void Set()
        {
            manualResetEvent.Set();
        }

        public bool Check()
        {
            return manualResetEvent.WaitOne(0, false);         
        }

    }
}
