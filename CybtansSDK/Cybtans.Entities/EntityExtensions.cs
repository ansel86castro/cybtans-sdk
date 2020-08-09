using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cybtans.Entities
{
    public static class EntityUtilities
    {
        public static Dictionary<object, object> ToDictionary(object obj) => (Dictionary<object, object>)ToData(obj);
        public static object ToData(object obj) => ToData(obj, new HashSet<object>());
        public static object ToData(object obj, HashSet<object> visited)
        {
            if (obj == null)
                return null;

            var type = obj.GetType();
            if (IsEncodable(type) || IsPrimitiveArray(type))
                return obj;

            if (obj is IEnumerable enumerable)
            {
                if (obj is IDictionary dic)
                {
                    return ToDataDictionary(dic, visited);
                }
                else if (obj is Array array)
                {
                    return ToDataArray(array, visited);
                }
                else if (obj is ICollection list)
                {
                    return ToDataList(list, visited);
                }
                else
                {
                    return ToDataList(enumerable, visited);
                }
            }


            return ToDataDictionary(obj, type, visited);

        }

        private static object ToDataList(ICollection list, HashSet<object> visited)
        {
            var encodedList = new List<object>(list.Count);
            foreach (var item in list)
            {               
                encodedList.Add(ToData(item, visited));
            }
            return encodedList;
        }

        private static object ToDataList(IEnumerable list, HashSet<object> visited)
        {
            var encodedList = new List<object>();
            foreach (var item in list)
            {
                encodedList.Add(ToData(item, visited));
            }
            encodedList.TrimExcess();
            return encodedList;
        }

        private static object ToDataArray(Array array, HashSet<object> visited)
        {
            var dataArray = Array.CreateInstance(array.GetType().GetElementType(), array.GetLength(0));
            for (int i = 0; i < dataArray.GetLength(0); i++)
            {
                dataArray.SetValue(ToData(array.GetValue(i), visited), i);
            }
            return dataArray;
        }

        private static object ToDataDictionary(IDictionary dic, HashSet<object> visited)
        {
            var encodedDic = new Dictionary<object, object>(dic.Count);
            foreach (DictionaryEntry entry in dic)
            {               
                encodedDic.Add(entry.Key, ToData(entry.Value, visited));
            }
            return encodedDic;
        }

        private static Dictionary<object, object> ToDataDictionary(object obj, Type type, HashSet<object> visited)
        {
            if (visited.Contains(obj))
                return null;

            visited.Add(obj);
            var data = new Dictionary<object, object>();
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);            


            foreach (var p in props)
            {
                if (!p.CanRead)
                    continue;
                
                var attr = p.GetCustomAttribute<EventDataAttribute>(true);
                if (attr == null)
                    continue;

                var propValue = p.GetValue(obj);                
                var value = ToData(propValue, visited);
                
                data.Add(p.Name, value);
            }

            return data;
        }

        private static bool IsEncodable(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(DateTime) || type == typeof(Guid);
        }

        private static bool IsPrimitiveArray(Type type)
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                return IsEncodable(elementType);
            }
            return false;
        }     
    }

}
