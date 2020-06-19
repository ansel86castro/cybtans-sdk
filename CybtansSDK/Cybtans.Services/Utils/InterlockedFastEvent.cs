// TODO: have a better way to define this....
#if UNITY_IPHONE
#define MANUALEVENT
#else
#define INTERLOCKED
#endif

using System;
using System.Collections.Generic;
using System.Threading;

namespace Cybtans.Services.Utils
{
    public class InterlockedFastEvent : IFastEvent
    {
        protected int counter = 0;

        public void Set()
        {
            Interlocked.Exchange(ref counter, 1);
        }

        public bool Check()
        {
            // we are trying to make this fast, if we lock each frame then we are just doing too much work
            return Interlocked.CompareExchange(ref counter, 0, 1) == 1;
        }

    }
}
