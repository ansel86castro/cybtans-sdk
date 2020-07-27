using System;

namespace Cybtans.Tests.Core.Xunit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DependsOnAttribute : Attribute
    {
        public string[] Methods { get; set; }

        public DependsOnAttribute(params string[] methods)
        {
            Methods = methods;
        }
    }


}
