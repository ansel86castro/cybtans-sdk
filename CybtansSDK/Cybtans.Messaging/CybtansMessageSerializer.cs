#nullable enable

using Cybtans.Serialization;
using System;

namespace Cybtans.Messaging
{
    public class CybtansMessageSerializer : IMessageSerializer
    {
        public string ContentType => BinarySerializer.MEDIA_TYPE;

        public T Deserialize<T>(ReadOnlyMemory<byte> memory)
        {
            return BinaryConvert.Deserialize<T>(memory.Span);
        }

        public byte[] Serialize(object obj)
        {
            return BinaryConvert.Serialize(obj);
        }
    }
}
