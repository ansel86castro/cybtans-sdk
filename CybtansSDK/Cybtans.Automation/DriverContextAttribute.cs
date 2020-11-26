using System;

namespace Cybtans.Automation
{
    /// <summary>
    /// Defined a Driver Context
    /// </summary>
    public class DriverContextAttribute : Attribute
    {
        public DriverContextAttribute(string context)
        {
            DriverName = context;
        }

        public string DriverName { get; }

        public int MaxReference { get; set; }

        public bool WaitForBarrier { get; set; }

        public bool DisableNavigation { get; set; }
    }
}
