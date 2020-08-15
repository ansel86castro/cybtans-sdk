using Cybtans.AspNetCore;
using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text;

#nullable enable

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CybtansFormatterServiceCollectionExtensions
    {
        public static void AddCybtansFormatter(this IMvcBuilder builder, Encoding? encoding = null)
        {
            builder.AddMvcOptions(options => options.AddCytansFormatter(encoding));
        }

        public static void AddCytansFormatter(this MvcOptions options, Encoding? encoding = null)
        {
            options.InputFormatters.Add(new BinaryInputFormatter(encoding ?? BinarySerializer.DefaultEncoding));
            options.OutputFormatters.Add(new BinaryOutputFormatter(encoding ?? BinarySerializer.DefaultEncoding));
            options.FormatterMappings.SetMediaTypeMappingForFormat("cybtans", MediaTypeHeaderValue.Parse(BinarySerializer.MEDIA_TYPE));
        }

        public static void AddAuthenticatedHttpHandler(this IServiceCollection services)
        {
            services.AddTransient<HttpClientAuthorizationHandler>();
        }
    }
}
