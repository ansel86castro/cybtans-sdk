using Cybtans.Refit;
using Cybtans.Serialization;
using Cybtans.Test.Domain.EF;
using Cybtans.Test.RestApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Cybtans.Tests.Entities.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;
using Xunit;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Cybtans.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using SQLitePCL;
using Microsoft.AspNetCore.TestHost;
using Cybtans.Tests.Services;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Cryptography;

namespace Cybtans.Tests.Integrations
{
    public class IntegrationFixture: BaseIntegrationFixture<Startup>
    {
        public EntityEventDelegateHandler<OrderMessageHandler> OrderEvents { get; private set; }

        public IntegrationFixture()
        {
            Claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "admin"),
                new Claim(JwtClaimTypes.Name, "admin"),
                new Claim(JwtRegisteredClaimNames.Email, "admin@gmail.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "test"),
                new Claim(JwtClaimTypes.Scope, "test"),
                new Claim("tenant", Guid.NewGuid().ToString())
            };

            OrderEvents = new EntityEventDelegateHandler<OrderMessageHandler>();
        }
        

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddInternalMessageQueue();
            
            var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(EntityEventDelegateHandler<OrderMessageHandler>));
            if(descriptor != null)
            {
                services.Remove(descriptor);
                services.AddSingleton(OrderEvents);
            }

            var sp= services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AdventureContext>();

                db.Database.EnsureCreated();

                RepositoryFixture.Seed(db).Wait();
            }
        }        
    }
}
