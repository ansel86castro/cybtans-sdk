using Cybtans.Serialization;
using Microsoft.IO;
using System.Buffers;
using System.Text;

namespace Cybtans.Common
{
    public class CybtansContentSerializer : IHttpContentSerializer, IDisposable
    {
        const int BufferSize = 16 * 1024;

        private readonly Encoding _encoding;
        private ThreadLocal<BinarySerializer> _serializerStore = new ThreadLocal<BinarySerializer>(() => new BinarySerializer());
        private readonly RecyclableMemoryStreamManager _streamManager = new RecyclableMemoryStreamManager();

        public CybtansContentSerializer(Encoding? encoding = null)
        {
            _encoding = encoding ?? BinarySerializer.DefaultEncoding;
        }

        private BinarySerializer Serializer => _serializerStore.Value ?? throw new InvalidOperationException("Unable to instantiate a BinarySerializer");

        public string ContentType => BinarySerializer.MEDIA_TYPE;

        public T FromUtf8Bytes<T>(byte[] array) where T : class
        {
            return Serializer.Deserialize<T>(array);
        }

        public async Task<T> FromStreamAsync<T>(Stream stream) where T : class
        {
            if (stream.CanSeek)
            {
                return Serializer.Deserialize<T>(stream);
            }

            using var memStream = _streamManager.GetStream();
            await stream.CopyToAsync(memStream);
            memStream.Position = 0;
            return Serializer.Deserialize<T>(memStream);
        }

        public IMemoryOwner<byte> ToMemory<T>(T obj) where T : class
        {
            var memStream = _streamManager.GetStream();
            Serializer.Serialize(memStream, obj);
            memStream.Position = 0;
            return new RecycableMemoryOwner(memStream);
        }

        public byte[] ToUtf8Bytes<T>(T obj) where T : class
        {
            return Serializer.Serialize(obj);
        }

        public void Dispose()
        {
            _serializerStore.Dispose();
        }

        class RecycableMemoryOwner : IMemoryOwner<byte>
        {
            private readonly MemoryStream _stream;
            private Memory<byte> _memory;

            public RecycableMemoryOwner(MemoryStream stream)
            {
                _stream = stream;
                if (!stream.TryGetBuffer(out var segment))
                    throw new InvalidOperationException();

                _memory = new Memory<byte>(segment.Array, segment.Offset, segment.Count);
            }

            public Memory<byte> Memory => _memory;

            public void Dispose()
            {
                _stream.Dispose();
            }
        }

    }
}