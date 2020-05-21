using Cybtans.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Cybtans.AspNetCore
{
    public static class CybtansFormatter
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
    }
}
