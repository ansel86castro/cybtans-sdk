using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Cybtans.Web
{
    public static class CybtansFormatter
    {
        public const string MEDIA_TYPE = "application/x-cybtans";

        public static void AddCybtansFormatter(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options => options.AddCytansFormatter());
        }

        public static void AddCytansFormatter(this MvcOptions options)
        {
            options.InputFormatters.Add(new BinaryInputFormatter());
            options.OutputFormatters.Add(new BinaryOutputFormatter());
            options.FormatterMappings.SetMediaTypeMappingForFormat("cybtans", MediaTypeHeaderValue.Parse(MEDIA_TYPE));
        }
    }
}
