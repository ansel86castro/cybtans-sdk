using Cybtans.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        
        public CybtansModelBinder()
        {            
        }

        private async Task<object> DeserializeBinary(Stream source, ModelBindingContext bindingContext)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                await source.CopyToAsync(stream).ConfigureAwait(false);
                stream.Position = 0;

                var obj = Serializer.Value.Deserialize(stream, bindingContext.ModelType);
                return obj;
            }
        }
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = bindingContext.HttpContext.Request;
            if (request.ContentType != null)
            {
                var contentType = MediaTypeHeaderValue.Parse(request.ContentType);
                if (contentType.MediaType == BinarySerializer.MEDIA_TYPE)
                {
                    bindingContext.Result = ModelBindingResult.Success(await DeserializeBinary(request.Body, bindingContext));
                    return;
                }
                else if (contentType.MediaType == "multipart/form-data")
                {
                    object obj = null;

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

                        if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                        {
                            //json body
                            if (contentType?.MediaType == "application/json")
                            {
                                var json = await section.ReadAsStringAsync();
                                obj = JsonConvert.DeserializeObject(json, bindingContext.ModelType);
                            }
                            else if (contentType?.MediaType == BinarySerializer.MEDIA_TYPE)
                            {
                                obj = await DeserializeBinary(section.Body, bindingContext);
                            }
                            else
                            {
                                obj ??= Activator.CreateInstance(bindingContext.ModelType);

                                //form value
                                var formData = await section.AsFormDataSection().GetValueAsync();
                                var formReader = new FormReader(formData);

                                foreach (var value in formReader.ReadForm())
                                {
                                    var prop = bindingContext.ModelMetadata.Properties[value.Key];
                                    if (prop.ModelType != typeof(string))
                                    {
                                        var v = Convert.ChangeType(value.Value.ToString(), prop.UnderlyingOrModelType);
                                        prop.PropertySetter(obj, v);
                                    }
                                    else
                                    {
                                        prop.PropertySetter(obj, value.Value.ToString());
                                    }
                                }
                            }
                        }
                        else if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                        {
                            var stream = new MemoryStream();
                            await section.AsFileSection().FileStream.CopyToAsync(stream);
                            if (bindingContext.ModelType == typeof(Stream))
                            {
                                obj = stream;
                            }
                            else
                            {
                                obj ??= Activator.CreateInstance(bindingContext.ModelType);

                                if (obj is IReflectorMetadataProvider reflectorMetadata)
                                {
                                    reflectorMetadata.SetValue(contentDisposition.Name.Value, stream);
                                }
                                else
                                {
                                    bindingContext.ModelMetadata.Properties[contentDisposition.Name.Value].PropertySetter(obj, stream);
                                }
                            }
                        }

                        section = await reader.ReadNextSectionAsync();
                    }

                    bindingContext.Result = ModelBindingResult.Success(obj);
                    return;
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    return;
                }
            }
            else if (bindingContext.ModelType == typeof(Stream))
            {                                
                bindingContext.Result = ModelBindingResult.Success(request.BodyReader.AsStream());
                return;
            }

            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}
