using Cybtans.Serialization;
using Refit;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Refit
{
    public class CybtansContentSerializer : IContentSerializer
    {
        static ThreadLocal<BinarySerializer> Serializer = new ThreadLocal<BinarySerializer>(() => new BinarySerializer(Encoding.UTF8));

        private readonly Encoding _encoding;
        private readonly string _mediaType;
        private readonly IContentSerializer _defaultSerializer;

        private CybtansContentSerializer(Encoding encoding, IContentSerializer defaultSerializer)
        {
            _encoding = encoding;
            _mediaType = $"{BinarySerializer.MEDIA_TYPE}; charset={_encoding.WebName}";
            _defaultSerializer = defaultSerializer;
        }

        public CybtansContentSerializer(IContentSerializer defaultSerializer) : this(Encoding.UTF8, defaultSerializer) { }

        public CybtansContentSerializer() : this(Encoding.UTF8, new SystemTextJsonContentSerializer()) { }

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            if (content.Headers.ContentType != null && content.Headers.ContentType.MediaType != BinarySerializer.MEDIA_TYPE)
            {
                return await _defaultSerializer.DeserializeAsync<T>(content).ConfigureAwait(false);
            }

            var serializer = _encoding == Encoding.UTF8 ? Serializer.Value : new BinarySerializer(_encoding);
            return serializer.Deserialize<T>(await content.ReadAsStreamAsync().ConfigureAwait(false));        
        }

        public Task<HttpContent> SerializeAsync<T>(T item)
        {
            var serializer = _encoding == Encoding.UTF8 ? Serializer.Value : new BinarySerializer(_encoding);

            var bytes = serializer.Serialize(item);

            var httpContent = new ByteArrayContent(bytes);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse(_mediaType);
            httpContent.Headers.ContentLength = bytes.Length;

            return Task.FromResult<HttpContent>(httpContent);
        }
        
    }
}
