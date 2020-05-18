using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Web
{
    public class BinaryOutputFormatter : OutputFormatter
    {
        Encoding _encoding;
        string _mediaType;
        public BinaryOutputFormatter() : this(Encoding.UTF8) { }

        public BinaryOutputFormatter(Encoding encoding)
        {
            _encoding = encoding;
            _mediaType = $"{BinarySerializer.MEDIA_TYPE} charset={_encoding.WebName}";
            SupportedMediaTypes.Add(_mediaType);            
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return true;
        }

        protected override bool CanWriteType(Type type)
        {
            return true;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;            

            var serializer = new BinarySerializer();

            var data = serializer.Serialize(context.Object);            
            
            response.ContentType =_mediaType;
            response.ContentLength = data.Length;

            await response.Body.WriteAsync(data, 0, data.Length);
        }
    }
}
