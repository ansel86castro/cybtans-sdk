using System.Collections.Generic;

namespace Cybtans.Serialization
{
    public static class ReflectorMetadataProviderExtensions
    {
        public static IReflectorMetadataProvider Merge(this IReflectorMetadataProvider obj, IDictionary<int, object> data)
        {
            var accesor = obj.GetAccesor();
            foreach (var kv in data)
            {
                var code = kv.Key;
                var type = accesor.GetPropertyType(code);
                accesor.SetValue(obj, code, BinaryConvert.ConvertTo(type, kv.Value));
            }

            return obj;
        }

        public static IReflectorMetadataProvider Merge(this IReflectorMetadataProvider obj, IDictionary<string, object> data)
        {
            var accesor = obj.GetAccesor();
            foreach (var kv in data)
            {
                var code = accesor.GetPropertyCode(kv.Key);
                var type = accesor.GetPropertyType(code);
                accesor.SetValue(obj, code, BinaryConvert.ConvertTo(type, kv.Value));
            }

            return obj;
        }

        public static T Merge<T>(this T obj, IDictionary<string, object> data)
            where T: IReflectorMetadataProvider
        {
            var accesor = obj.GetAccesor();
            foreach (var kv in data)
            {
                var code = accesor.GetPropertyCode(kv.Key);
                var type = accesor.GetPropertyType(code);
                accesor.SetValue(obj, code, BinaryConvert.ConvertTo(type, kv.Value));
            }

            return obj;
        }

        public static T Merge<T>(this T obj, IDictionary<int, object> data)
             where T : IReflectorMetadataProvider
        {
            var accesor = obj.GetAccesor();
            foreach (var kv in data)
            {
                var code = kv.Key;
                var type = accesor.GetPropertyType(code);
                accesor.SetValue(obj, code, BinaryConvert.ConvertTo(type, kv.Value));
            }

            return obj;
        }

        public static Dictionary<string, object> ToNamedDictionary(this IReflectorMetadataProvider obj)
        {
            var data = new Dictionary<string, object>();
            var accesor = obj.GetAccesor();
            
            foreach (var code in accesor.GetPropertyCodes())
            {
                data.Add(accesor.GetPropertyName(code), accesor.GetValue(obj, code));
            }

            return data;
        }

        public static Dictionary<int, object> ToCodedDictionary(this IReflectorMetadataProvider obj)
        {
            var data = new Dictionary<int, object>();
            var accesor = obj.GetAccesor();

            foreach (var code in accesor.GetPropertyCodes())
            {
                data.Add(code, accesor.GetValue(obj, code));
            }

            return data;
        }

        public static T Clone<T>(this T obj) where T : IReflectorMetadataProvider, new()
        {
            var accesor = obj.GetAccesor();
            T clone = new T();

            foreach (var code in accesor.GetPropertyCodes())
            {
                accesor.SetValue(clone, code, accesor.GetValue(obj, code));
            }

            return clone;
        }

        public static T Map<T>(this IReflectorMetadataProvider obj) where T : IReflectorMetadataProvider, new()
        {
            var accesor = obj.GetAccesor();
            T clone = new T();
            var targetAccesor = clone.GetAccesor();

            foreach (var code in accesor.GetPropertyCodes())
            {
                targetAccesor.SetValue(clone, code, accesor.GetValue(obj, code));
            }

            return clone;
        }

        public static object GetValue(this IReflectorMetadataProvider obj, string propertyName)
        {
            var accesor = obj.GetAccesor();
            return accesor.GetValue(obj, accesor.GetPropertyCode(propertyName));
        }

        public static void SetValue(this IReflectorMetadataProvider obj, string propertyName, object value)
        {
            var accesor = obj.GetAccesor();
            accesor.SetValue(obj, accesor.GetPropertyCode(propertyName), value);
        }
    }
}
