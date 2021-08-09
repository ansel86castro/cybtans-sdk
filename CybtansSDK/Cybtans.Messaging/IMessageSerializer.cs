#nullable enable

using System;

namespace Cybtans.Messaging
{
    public interface IMessageSerializer
    {
        string ContentType { get; }

        T Deserialize<T>(ReadOnlyMemory<byte> memory);

        byte[] Serialize(object obj);
    }
}
