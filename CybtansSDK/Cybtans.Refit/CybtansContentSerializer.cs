using Cybtans.Serialization;
using Refit;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Refit
{
    public class CybtansContentSerializer : IContentSerializer
    {
        Encoding _encoding;
        string _mediaType;

        public CybtansContentSerializer(Encoding encoding)
        {
            _encoding = encoding;
            _mediaType = $"{BinarySerializer.MEDIA_TYPE} charset={_encoding.WebName}";
        }

        public CybtansContentSerializer() : this(Encoding.UTF8) { }

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            var serializer = new BinarySerializer(_encoding);
            return serializer.Deserialize<T>(await content.ReadAsStreamAsync());            
        }

        public async Task<HttpContent> SerializeAsync<T>(T item)
        {
            using MemoryStream stream = new MemoryStream();
            var serializer = new BinarySerializer(_encoding);
            serializer.Serialize(stream, item);

            var httpContent = new StreamContent(stream);            

            httpContent.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);
            httpContent.Headers.ContentLength = stream.Length;
            return httpContent;
        }
    }
}
