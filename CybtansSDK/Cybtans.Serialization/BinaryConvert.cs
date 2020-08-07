using System;
using System.IO;
using System.Threading;

namespace Cybtans.Serialization
{
    public static class BinaryConvert
    {
        private static ThreadLocal<BinarySerializer> serializer = new ThreadLocal<BinarySerializer>(() => new BinarySerializer());        public static byte[] Serialize(object obj) => new BinarySerializer().Serialize(obj);

        public static void Serialize(Stream stream, object obj) => serializer.Value.Serialize(stream, obj);

        public static T Deserialize<T>(Stream stream) => (T)Deserialize(stream, typeof(T));

        public static T Deserialize<T>(byte[] bytes) => (T)Deserialize(bytes, typeof(T));

        public static object Deserialize(byte[] bytes) => Deserialize(bytes, null);

        public static object Deserialize(Stream stream) => Deserialize(stream, null);

        public static object Deserialize(byte[] bytes, Type? type)
        {
            using var stream = new MemoryStream(bytes);
            return Deserialize(stream, type);
        }

        public static object? Deserialize(Stream stream, Type? type) => serializer.Value.Deserialize(stream, type);

        public static object ConvertTo(Type type, object value)
        {
            if (value != null && type != null)
            {
                var valueType = value.GetType();
                if (valueType != type)
                {
                    if (valueType.IsPrimitive && IsPrimitiveNullable(type, out var nullableType))
                    {
                        if (nullableType != valueType)
                        {
                            value = System.Convert.ChangeType(value, nullableType);
                        }
                    }
                    else if (!type.IsAssignableFrom(valueType) && value is IConvertible convertible)
                    {
                        value = convertible.ToType(type, null);
                    }
                }
            }

            return value;
        }

        private static bool IsPrimitiveNullable(Type targetType, out Type converType)
        {
            converType = null;
            if (targetType == typeof(bool?))
            {
                converType = typeof(bool);
            }
            else if (targetType == typeof(byte?))
            {
                converType = typeof(byte);
            }
            else if (targetType == typeof(sbyte?))
            {
                converType = typeof(sbyte);
            }
            else if (targetType == typeof(short?))
            {
                converType = typeof(short);
            }
            else if (targetType == typeof(ushort?))
            {
                converType = typeof(ushort);
            }
            else if (targetType == typeof(int?))
            {
                converType = typeof(int);
            }
            else if (targetType == typeof(uint?))
            {
                converType = typeof(uint);
            }
            else if (targetType == typeof(long?))
            {
                converType = typeof(long);
            }
            else if (targetType == typeof(ulong?))
            {
                converType = typeof(ulong);
            }
            else if (targetType == typeof(float?))
            {
                converType = typeof(float);
            }
            else if (targetType == typeof(double?))
            {
                converType = typeof(double);
            }
            else if (targetType == typeof(decimal?))
            {
                converType = typeof(decimal);
            }
            else if (targetType == typeof(char?))
            {
                converType = typeof(char);
            }

            return converType != null;
        }
    }
}
