using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.AspNetCore
{
    public class BinaryOutputFormatter : OutputFormatter
    {
        static ThreadLocal<BinarySerializer> Serializer = new ThreadLocal<BinarySerializer>(() => new BinarySerializer());

        private readonly Encoding _encoding;
        private readonly string _mediaType;
       
        public BinaryOutputFormatter() : this(BinarySerializer.DefaultEncoding) { }

        public BinaryOutputFormatter(Encoding encoding)
        {
            _encoding = encoding;
            _mediaType = $"{BinarySerializer.MEDIA_TYPE}; charset={_encoding.WebName}";
            SupportedMediaTypes.Add(_mediaType);
            SupportedMediaTypes.Add(BinarySerializer.MEDIA_TYPE);
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            context.ContentType = _mediaType;
            return true;
        }

        protected override bool CanWriteType(Type type)
        {
            return true;
        }     

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            
            var serializer = _encoding == BinarySerializer.DefaultEncoding ? Serializer.Value : new BinarySerializer(_encoding);

            var stream = new MemoryStream();
            serializer.Serialize(stream,context.Object);
            stream.Position = 0;

            response.ContentType = _mediaType;
            response.ContentLength = stream.Length;

            return stream.CopyToAsync(response.Body);
        }
    }
}
