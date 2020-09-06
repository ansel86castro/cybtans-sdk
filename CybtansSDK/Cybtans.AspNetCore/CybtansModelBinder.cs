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
using System.Security.Cryptography.X509Certificates;
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
                                var sectionValue = await section.AsFormDataSection().GetValueAsync();                                
                                var formReader = new FormReader(sectionValue);
                                var form = formReader.ReadForm();

                                if (form.Count == 0)
                                {
                                    if(contentDisposition.Name != null)
                                    {
                                        SetValue(bindingContext, obj, sectionValue, contentDisposition.Name.Value);                                        
                                    }
                                }
                                else
                                {
                                    foreach (var value in form)
                                    {
                                        SetValue(bindingContext, obj, value.Value.ToString(), value.Key);                                        
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
                                SetValue(bindingContext, obj, stream, contentDisposition.Name.Value);
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
                bindingContext.Result = ModelBindingResult.Success(request.Body);
                return;
            }

            bindingContext.Result = ModelBindingResult.Failed();
        }

        public static string Pascal(string s)
        {
            var sections = s.Split('_');
            StringBuilder sb = new StringBuilder();
            foreach (var part in sections)
            {

                for (int i = 0; i < part.Length; i++)
                {
                    var c = part[i];
                    if (i == 0)
                    {
                        sb.Append(char.ToUpperInvariant(c));
                    }
                    else if (i < part.Length - 1 && char.IsLower(part[i - 1]) && char.IsUpper(c))
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        sb.Append(char.ToLowerInvariant(c));
                    }

                }
            }

            var result = sb.ToString();
            if (result.All(x => char.IsDigit(x)))
            {
                result = "_" + result;
            }

            return result;
        }
    
        private static void SetValue(ModelBindingContext bindingContext, object obj, string value, string property)
        {
            if (obj is IReflectorMetadataProvider reflectorMetadata)
            {
                var accesor = reflectorMetadata.GetAccesor();
                var propCode = accesor.GetPropertyCode(Pascal(property));
                var propType = accesor.GetPropertyType(propCode);
                propType = Nullable.GetUnderlyingType(propType) ?? propType;

                reflectorMetadata.SetValue(Pascal(property),
                    propType != typeof(string) ?
                    Convert.ChangeType(value, propType) :
                    value);
            }
            else
            {
                var prop = bindingContext.ModelMetadata.Properties.FirstOrDefault(x => x.Name.ToUpperInvariant() == property.ToUpperInvariant());
                prop?.PropertySetter(obj,
                   prop.ModelType != typeof(string) ?
                   Convert.ChangeType(value, prop.UnderlyingOrModelType) :
                   value);
            }
        }

        private static void SetValue(ModelBindingContext bindingContext, object obj, object value, string property)
        {
            if (obj is IReflectorMetadataProvider reflectorMetadata)
            {
                var accesor = reflectorMetadata.GetAccesor();
                var propCode = accesor.GetPropertyCode(Pascal(property));
                var propType = accesor.GetPropertyType(propCode);
                propType = Nullable.GetUnderlyingType(propType) ?? propType;

                reflectorMetadata.SetValue(Pascal(property), value);
            }
            else
            {
                var prop = bindingContext.ModelMetadata.Properties.FirstOrDefault(x => x.Name.ToUpperInvariant() == property.ToUpperInvariant());
                prop?.PropertySetter(obj, value);
            }
        }
    }
}
