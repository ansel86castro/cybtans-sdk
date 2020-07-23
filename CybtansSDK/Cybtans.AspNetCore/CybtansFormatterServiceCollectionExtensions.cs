using Cybtans.AspNetCore;
using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CybtansFormatterServiceCollectionExtensions
    {
        public static void AddCybtansFormatter(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options => options.AddCytansFormatter());
        }

        public static void AddCytansFormatter(this MvcOptions options)
        {
            options.InputFormatters.Add(new BinaryInputFormatter());
            options.OutputFormatters.Add(new BinaryOutputFormatter());
            options.FormatterMappings.SetMediaTypeMappingForFormat("cybtans", MediaTypeHeaderValue.Parse(BinarySerializer.MEDIA_TYPE));
        }

        public static void AddAuthenticatedHttpHandler(this IServiceCollection services)
        {
            services.AddTransient<HttpClientAuthorizationHandler>();
        }
    }
}
