using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cybtans.AspNetCore;

namespace @{NAMESPACE}
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
            services.AddHttpContextAccessor();

           #region swagger
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "@{SERVICE}", Version = "v1" });
                c.OperationFilter<SwachBuckleOperationFilters>();               
            });

            #endregion

            #region Authentication

             services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                //Replace Authority with your authority url
                options.Authority = "http://identity.restapi";
                //Replace Audience with your audience 
                options.Audience = "http://identity.restapi/resources";

                options.RequireHttpsMetadata = false;                
                options.SaveToken = true;                                 
                options.Validate();               
            });

            services.AddAuthorization();

            #endregion

            #region Cors

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                       builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(Configuration.GetValue<string>("AllowedHosts").Split(','))
                        .AllowAnyHeader()
                        .AllowAnyMethod();                        
                    });
            });

            #endregion
         
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

            app.UseCors();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "@{SERVICE} V1");
                c.EnableFilter();
                c.EnableDeepLinking();
                c.ShowCommonExtensions();                                
            });        

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
