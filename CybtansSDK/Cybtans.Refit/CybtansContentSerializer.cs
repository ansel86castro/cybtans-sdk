using Cybtans.Serialization;
using Refit;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Refit
{
    public class CybtansContentSerializer : IContentSerializer
    {
        private readonly Encoding _encoding;
        private readonly string _mediaType;
        private readonly IContentSerializer _defaultSerializer;

        public CybtansContentSerializer(Encoding encoding, IContentSerializer defaultSerializer)
        {
            _encoding = encoding;
            _mediaType = $"{BinarySerializer.MEDIA_TYPE}; charset={_encoding.WebName}";
            _defaultSerializer = defaultSerializer;
        }

        public CybtansContentSerializer(IContentSerializer defaultSerializer) : this(Encoding.UTF8, defaultSerializer) { }

        public CybtansContentSerializer() : this(Encoding.UTF8, new SystemTextJsonContentSerializer()) { }

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            var str = await content.ReadAsStringAsync();

            if(content.Headers.ContentType.MediaType != BinarySerializer.MEDIA_TYPE)
            {
                return await _defaultSerializer.DeserializeAsync<T>(content);
            }

            var serializer = new BinarySerializer(_encoding);
            return serializer.Deserialize<T>(await content.ReadAsStreamAsync());            
        }

        public async Task<HttpContent> SerializeAsync<T>(T item)
        {            
            var serializer = new BinarySerializer(_encoding);
            var bytes = serializer.Serialize(item);

            var httpContent = new ByteArrayContent(bytes);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse(_mediaType);
            httpContent.Headers.ContentLength = bytes.Length;
            return httpContent;
        }
        
    }
}
