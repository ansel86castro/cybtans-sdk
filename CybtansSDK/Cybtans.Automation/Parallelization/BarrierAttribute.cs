using System;

#nullable enable

namespace Cybtans.Automation
{
    public class BarrierAttribute : Attribute
    {
        public BarrierAttribute() { }

        public BarrierAttribute(string barrierName)
        {
            BarrierName = barrierName;
        }

        public string? BarrierName { get; }

        public int WaitCount { get; set; } = 1;
    }

}

#nullable restore
