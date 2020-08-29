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
                            if (contentType?.MediaType == "application/json")
                            {
                                //json body
                                var json = await section.ReadAsStringAsync();                              
                                obj = JsonConvert.DeserializeObject(json, bindingContext.ModelType);
                            }
                            else
                            {
                                if (obj == null)
                                {
                                    obj = Activator.CreateInstance(bindingContext.ModelType);
                                }

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
                        else if(MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                        {
                            var stream =new MemoryStream();
                            await section.AsFileSection().FileStream.CopyToAsync(stream);

                            if (obj == null)
                            {
                                obj = Activator.CreateInstance(bindingContext.ModelType);
                            }

                            if(obj is IReflectorMetadataProvider reflectorMetadata)
                            {                                
                                reflectorMetadata.SetValue(contentDisposition.Name.Value, stream);
                            }
                            else
                            {
                                bindingContext.ModelMetadata.Properties[contentDisposition.Name.Value].PropertySetter(obj, stream);
                            }
                        }

                        section = await reader.ReadNextSectionAsync();
                    }

                    bindingContext.Result = ModelBindingResult.Success(obj);
                    return;
                }
            }

            throw new NotImplementedException();

        }
    }
}
