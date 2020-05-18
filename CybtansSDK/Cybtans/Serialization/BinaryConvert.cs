#nullable enable

using System;
using System.IO;

namespace Cybtans.Serialization
{
    public static class BinaryConvert
    {
        public static byte[] Serialize(object obj) => new BinarySerializer().Serialize(obj);

        public static void Serialize(Stream stream, object obj) => new BinarySerializer().Serialize(stream, obj);

        public static T Deserialize<T>(Stream stream) => (T)Deserialize(stream, typeof(T));

        public static T Deserialize<T>(byte[] bytes) => (T)Deserialize(bytes, typeof(T));

        public static object? Deserialize(byte[] bytes) => Deserialize(bytes, null);

        public static object? Deserialize(Stream stream) => Deserialize(stream, null);

        public static object? Deserialize(byte[] bytes, Type? type)
        {
            using var stream = new MemoryStream(bytes);
            return Deserialize(stream, type);
        }

        public static object? Deserialize(Stream stream, Type? type) => new BinarySerializer().Deserialize(stream, type);
    }
}
