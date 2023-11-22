using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Authentication.AspNetCore
{
    public class ServiceCollectionExtension
    {
        public IServiceCollection AddCybtansAuthentication(IServiceCollection services, Action<JwtBearerOptions> configure)
        {
            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>(sp=>
                new ConfigureJwtBearerOptions(sp.GetRequiredService<ICertificateRepository>(), configure));
            return services;
        }
    }
}
