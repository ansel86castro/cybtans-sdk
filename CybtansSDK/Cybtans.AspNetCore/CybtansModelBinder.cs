using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.AspNetCore
{
    public class CybtansModelBinder : IModelBinder
    {
        static ThreadLocal<BinarySerializer> Serializer = new ThreadLocal<BinarySerializer>(() => new BinarySerializer());

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {            
            var request = bindingContext.HttpContext.Request;
            if (request.ContentType != null)
            {
                var contentType = MediaTypeHeaderValue.Parse(request.ContentType);
                if (contentType.MediaType == BinarySerializer.MEDIA_TYPE)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await request.Body.CopyToAsync(stream).ConfigureAwait(false);
                        stream.Position = 0;

                        var obj = Serializer.Value.Deserialize(stream, bindingContext.ModelType);
                        bindingContext.Result = ModelBindingResult.Success(obj);
                    }
                    return;
                }
                else if (contentType.MediaType == "multipart/form-data")
                {
                    if (contentType.Boundary == null)
                        throw new InvalidOperationException("MultiPartBoundary not found");
                    var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

                    var reader = new MultipartReader(boundary, request.Body);
                    var section = await reader.ReadNextSectionAsync();

                    while (section != null)
                    {
                        ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
                        MediaTypeHeaderValue.TryParse(section.ContentType, out contentType);

                        if (contentDisposition == null)
                            continue;

                        if(contentType ==null || contentType.MediaType == "application/json")
                        {

                        }
                    }
                }
            }

            throw new NotImplementedException();

        }
    }
}
