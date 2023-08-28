using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Buffers;
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
        //64Kb
        const int MaxBufferLength = 256 * 1024;

        static ThreadLocal<BinarySerializer> Serializer = new ThreadLocal<BinarySerializer>(() => new BinarySerializer());        

        private readonly Encoding _encoding;
        private ArrayPool<byte> _pool;

        public BinaryInputFormatter() : this(BinarySerializer.DefaultEncoding) { }

        public BinaryInputFormatter(Encoding encoding)
        {
            _encoding = encoding;
            _pool = ArrayPool<byte>.Shared;            

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

            var serializer = _encoding == BinarySerializer.DefaultEncoding ? Serializer.Value! : new BinarySerializer(_encoding);
            
            byte[]? buffer = null;
            Stream? stream = null;
            object? result;

            try
            {
                if (request.ContentLength != null && request.ContentLength < MaxBufferLength)
                {
                    buffer = _pool.Rent((int)request.ContentLength);
                    stream = new MemoryStream(buffer, 0, buffer.Length, true);
                }
                else
                {
                    stream = new MemoryStream();
                }

                await request.Body.CopyToAsync(stream).ConfigureAwait(false);
                stream.Position = 0;                

                result = serializer.Deserialize(stream, type);                
            }
            finally
            {
                if (buffer != null)
                {
                    _pool.Return(buffer);
                }
                else
                {
                    stream?.Dispose();
                }
            }

            return await InputFormatterResult.SuccessAsync(result).ConfigureAwait(false);
        }

        
    }
}
