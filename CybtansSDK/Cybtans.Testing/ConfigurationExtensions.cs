using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Cybtans.Testing
{
    public static class ConfigurationExtensions
    {
        public static IOptions<T> GetOptions<T>(this IConfiguration configuration, string key)
           where T : class, new()
        {
            T options = new T();
            if (key != null)
            {
                configuration.Bind(key, options);
            }
            else
            {
                configuration.Bind(options);
            }
            return Options.Create(options);
        }

        public static IOptions<T> GetOptions<T>(this IConfiguration configuration)
         where T : class, new()
        {
            T options = new T();
            configuration.Bind(typeof(T).Name, options);
            return Options.Create(options);
        }
    }
}