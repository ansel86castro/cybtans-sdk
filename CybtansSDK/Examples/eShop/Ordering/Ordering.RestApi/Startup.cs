using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cybtans.AspNetCore;
using Cybtans.Entities;
using Cybtans.Entities.EntityFrameworkCore;
using Cybtans.Messaging.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Data;
using Ordering.Models;
using Ordering.Services;

namespace Ordering.RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            EfAsyncQueryExecutioner asyncQueryExecutioner = new EfAsyncQueryExecutioner();
            asyncQueryExecutioner.SetCurrent();

            // Register the Swagger services
            services.AddOpenApiDocument();

            services.AddScoped<OrdersService, OrdersServiceImpl>();

            services.AddDbContext<OrderingContext>();
            services.AddRepositories<OrderingContext>();
            services.AddMessageQueue(Configuration)
                .ConfigureSubscriptions(queue=>
                {
                    queue.Subscribe<EntityCreated<Order>, OrderCreatedProcessor>();
                });

            services.AddDbContextEventPublisher<OrderingContext>();

            services.AddControllers()
                .AddCybtansFormatter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi(); 
            app.UseSwaggerUi3();             

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
