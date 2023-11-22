using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;


namespace Cybtans.Authentication.AspNetCore
{
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly ICertificateRepository _keyProvider;
        private readonly Action<JwtBearerOptions> _configureOptions;

        public ConfigureJwtBearerOptions(            
            ICertificateRepository keyProvider,
            Action<JwtBearerOptions> configureOptions)
        {            
            _keyProvider = keyProvider;
            _configureOptions = configureOptions;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            var cert = _keyProvider.GetDefaultCertificate().Result ?? throw new InvalidOperationException("Certificate not found");            

            if (name == JwtBearerDefaults.AuthenticationScheme)
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "memogram",
                    ValidIssuer = "https://memogram.cybtans.com",
                    IssuerSigningKey = new RsaSecurityKey(cert.Certificate.GetRSAPublicKey()),
                    ClockSkew = TimeSpan.FromSeconds(5)
                };

                _configureOptions?.Invoke(options);

                options.Validate();
            }
        }       


        public void Configure(JwtBearerOptions options)
        {
            Configure(string.Empty, options);
        }
    }
}
