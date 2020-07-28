using System;

namespace Cybtans.Tests.Core.Xunit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestOrderAttribute : Attribute
    {
        public TestOrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }


}
