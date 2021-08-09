#nullable enable

using System;
using System.Text.Json;

namespace Cybtans.Messaging
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        public string ContentType => "application/json";

        public T Deserialize<T>(ReadOnlyMemory<byte> memory)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(memory.Span);  
        }

        public byte[] Serialize(object obj)
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(obj);
        }
    }
}
