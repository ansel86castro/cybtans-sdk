using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Entities.MongoDb
{
    public static class Utils
    {
        public static Dictionary<string, object?> ToDictionary(object value)
        {
            var properties = value.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            return properties.ToDictionary(p => p.Name, p => p.GetValue(value));
        }
    }
}
