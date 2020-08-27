using Cybtans.Serialization;
using Refit;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Refit
{
    public class CybtansContentSerializer : IContentSerializer
    {
        static ThreadLocal<BinarySerializer> Serializer = new ThreadLocal<BinarySerializer>(() => new BinarySerializer());        

        private readonly Encoding _encoding;
        private readonly string _mediaType;
        private readonly IContentSerializer _defaultSerializer;

        public CybtansContentSerializer(Encoding encoding, IContentSerializer defaultSerializer)
        {
            _encoding = encoding;
            _mediaType = $"{BinarySerializer.MEDIA_TYPE}; charset={_encoding.WebName}";
            _defaultSerializer = defaultSerializer;
        }

        public CybtansContentSerializer(IContentSerializer defaultSerializer) : this(BinarySerializer.DefaultEncoding, defaultSerializer) { }

        public CybtansContentSerializer() : this(BinarySerializer.DefaultEncoding, new SystemTextJsonContentSerializer()) { }

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            if (content.Headers.ContentType != null && content.Headers.ContentType.MediaType != BinarySerializer.MEDIA_TYPE)
            {
                var json = await content.ReadAsStringAsync();
                return await _defaultSerializer.DeserializeAsync<T>(content).ConfigureAwait(false);
            }

            BinarySerializer serializer;
            if (content.Headers.ContentType == null || content.Headers.ContentType.CharSet == BinarySerializer.DefaultEncoding.WebName)
            {
                serializer = Serializer.Value;
            }
            else
            {
                var encoding = content.Headers.ContentType.CharSet == _encoding.WebName ?
                    _encoding :
                    Encoding.GetEncoding(content.Headers.ContentType.CharSet);

                serializer = new BinarySerializer(encoding);
            }

            using MemoryStream stream = content.Headers?.ContentLength != null ?
                new MemoryStream((int)content.Headers.ContentLength) :
                new MemoryStream();

            await content.CopyToAsync(stream);
            stream.Position = 0;

            return serializer.Deserialize<T>(stream);
        }

        public Task<HttpContent> SerializeAsync<T>(T item)
        {
            var serializer = _encoding == BinarySerializer.DefaultEncoding ? Serializer.Value : new BinarySerializer(_encoding);

            using var stream = new MemoryStream();
            serializer.Serialize(stream, item);
            var bytes = stream.ToArray();

            var httpContent = new ByteArrayContent(bytes);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse(_mediaType);
            httpContent.Headers.ContentLength = bytes.Length;

            return Task.FromResult<HttpContent>(httpContent);
        }
        
    }
}
