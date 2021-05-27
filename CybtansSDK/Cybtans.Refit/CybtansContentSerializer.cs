using Cybtans.Serialization;
using Microsoft.IO;
using Refit;
using System;
using System.Buffers;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Refit
{
    public class CybtansContentSerializer : IHttpContentSerializer
    {      
        const string TAG = "CybtansContentSerializer";

        static ThreadLocal<BinarySerializer> Serializer = new ThreadLocal<BinarySerializer>(() => new BinarySerializer());        
        private readonly Encoding _encoding;
        private readonly string _mediaType;
        private readonly IHttpContentSerializer _defaultSerializer;      
        private static readonly RecyclableMemoryStreamManager _streamManager = new RecyclableMemoryStreamManager();

        public CybtansContentSerializer(Encoding encoding, IHttpContentSerializer defaultSerializer)
        {                       
            _encoding = encoding;
            _mediaType = $"{BinarySerializer.MEDIA_TYPE}; charset={_encoding.WebName}";
            _defaultSerializer = defaultSerializer;            
        }

        public CybtansContentSerializer(IHttpContentSerializer defaultSerializer) : this(BinarySerializer.DefaultEncoding, defaultSerializer) { }

        public CybtansContentSerializer() : this(BinarySerializer.DefaultEncoding, new SystemTextJsonContentSerializer()) { }

        public async Task<T> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (content.Headers.ContentType != null && content.Headers.ContentType.MediaType != BinarySerializer.MEDIA_TYPE)
            {                
                return await _defaultSerializer.FromHttpContentAsync<T>(content, cancellationToken).ConfigureAwait(false);
            }

            BinarySerializer serializer;
            if (content.Headers.ContentType == null || content.Headers.ContentType.CharSet == null || content.Headers.ContentType.CharSet == BinarySerializer.DefaultEncoding.WebName)
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

            MemoryStream stream = null;
            try
            {
                stream = content.Headers?.ContentLength != null ?
                                _streamManager.GetStream("CybtansContentSerializer", (int)content.Headers.ContentLength) :
                                _streamManager.GetStream("CybtansContentSerializer");

                await content.CopyToAsync(stream);
                stream.Position = 0;

                return serializer.Deserialize<T>(stream);
            }
            finally
            {
                stream?.Dispose();
            }
        }      

        public HttpContent ToHttpContent<T>(T item)
        {
            var serializer = _encoding == BinarySerializer.DefaultEncoding ? Serializer.Value : new BinarySerializer(_encoding);
         
            using var stream = _streamManager.GetStream();
            
            serializer.Serialize(stream, item);
            var bytes = stream.ToArray();

            var httpContent = new ByteArrayContent(bytes);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse(_mediaType);
            httpContent.Headers.ContentLength = bytes.Length;

            return httpContent;
        }

        public string GetFieldNameForProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.Name;
        }
    }
}
