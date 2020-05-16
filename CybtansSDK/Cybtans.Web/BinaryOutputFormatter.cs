using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Cybtans.Web
{
    public class BinaryOutputFormatter : OutputFormatter
    {
        public BinaryOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(CybtansFormatter.MEDIA_TYPE));            
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
            
            response.ContentType = CybtansFormatter.MEDIA_TYPE;
            response.ContentLength = data.Length;

            await response.Body.WriteAsync(data, 0, data.Length);
        }
    }
}
