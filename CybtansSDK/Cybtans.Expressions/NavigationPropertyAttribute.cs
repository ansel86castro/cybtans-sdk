using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions
{
    public class NavigationPropertyAttribute : Attribute
    {
        public string NavigationProperty { get; set; }

        public string Property { get; set; }
    }
}
