using System.Buffers;

namespace Cybtans.Common
{

    public interface IHttpContentSerializer
    {
        string ContentType { get; }
        IMemoryOwner<byte> ToMemory<T>(T obj) where T : class;
        Task<T> FromStreamAsync<T>(Stream stream) where T : class;
        T FromUtf8Bytes<T>(byte[] array) where T : class;
        byte[] ToUtf8Bytes<T>(T obj) where T : class;
    }
}