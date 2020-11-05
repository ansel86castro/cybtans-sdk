using System;

#nullable enable

namespace Cybtans.Automation
{
    public class WaitForBarrierAttribute : Attribute
    {
        public WaitForBarrierAttribute(string barrierName)
        {
            BarrierName = barrierName;
        }

        public string BarrierName { get; }
    }

}

#nullable restore
