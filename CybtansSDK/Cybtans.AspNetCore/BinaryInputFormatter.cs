using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.AspNetCore
{
    public class BinaryInputFormatter : InputFormatter
    {
        static ThreadLocal<BinarySerializer> Serializer = new ThreadLocal<BinarySerializer>(() => new BinarySerializer());

        private readonly Encoding _encoding;        

        public BinaryInputFormatter() : this(BinarySerializer.DefaultEncoding) { }

        public BinaryInputFormatter(Encoding encoding)
        {
            _encoding = encoding;            
            SupportedMediaTypes.Add($"{BinarySerializer.MEDIA_TYPE}; charset={_encoding.WebName}");
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
            await request.Body.CopyToAsync(stream).ConfigureAwait(false);
            stream.Position = 0;
            
            var serializer = _encoding ==BinarySerializer.DefaultEncoding ? Serializer.Value! : new BinarySerializer(_encoding);

            var result = serializer.Deserialize(stream, type);
            return await InputFormatterResult.SuccessAsync(result).ConfigureAwait(false);
        }

        
    }
}
