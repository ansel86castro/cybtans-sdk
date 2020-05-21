using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.AspNetCore
{
    public class BinaryInputFormatter : InputFormatter
    {
        private readonly Encoding _encoding;
        private readonly string _mediaType;

        public BinaryInputFormatter() : this(Encoding.UTF8) { }

        public BinaryInputFormatter(Encoding encoding)
        {
            _encoding = encoding;
            _mediaType = $"{BinarySerializer.MEDIA_TYPE}; charset={_encoding.WebName}";
            SupportedMediaTypes.Add(_mediaType);
            SupportedMediaTypes.Add(BinarySerializer.MEDIA_TYPE);
        }


        public override bool CanRead(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            return request.Method == "POST" || request.Method == "PUT";
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var type = context.ModelType;
            var request = context.HttpContext.Request;

            using MemoryStream stream = new MemoryStream();
            await request.Body.CopyToAsync(stream);
            stream.Position = 0;

            var serializer = new BinarySerializer(_encoding);
            var result = serializer.Deserialize(stream, type);
            return await InputFormatterResult.SuccessAsync(result);
        }
    }
}
