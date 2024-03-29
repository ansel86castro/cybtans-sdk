﻿using Cybtans.Test.RestApi;
using System;
using Microsoft.Extensions.DependencyInjection;
using Cybtans.Tests.Entities.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Cybtans.Tests.Services;
using System.Linq;
using Cybtans.Tests.Domain.EF;
using Cybtans.Tests.Clients;

namespace Cybtans.Tests.Integrations
{
    public class IntegrationFixture: BaseIntegrationFixture<Startup>
    {
        private static object @lock = new object();

        public EntityEventDelegateHandler<OrderMessageHandler> OrderEvents { get; private set; }

        public IntegrationFixture()
        {
            ClientAssembly = typeof(ClientServiceClient).Assembly;
            
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
        

        protected override void ConfigureServices(IServiceCollection services)
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

                lock (@lock)
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AdventureContext>();

                    if (db.Database.EnsureCreated())
                    {
                        RepositoryFixture.Seed(db).Wait();
                    }
                }
            }
        }        
    }
}
