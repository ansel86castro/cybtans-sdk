using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using FluentValidation.AspNetCore;
using Cybtans.AspNetCore;
using Cybtans.Entities.EntityFrameworkCore;
using Cybtans.Services.Extensions;

using @{SERVICE}.Domain.EntityFramework;
using @{SERVICE}.Services;
using @{SERVICE}.Domain;

namespace @{NAMESPACE}
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            AddSwagger(services);

            AddAuthentication(services);

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

            services.AddDbContext<@{SERVICE}Context>(
                builder => builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
            .AddUnitOfWork<@{SERVICE}Context>()
            .AddRepositories();

            services.AddAutoMapper(typeof(@{SERVICE}Stub));
         
            services
            .AddControllers(options => options.Filters.Add<HttpResponseExceptionFilter>())
            .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining(typeof(@{SERVICE}Stub)))
            .AddCybtansFormatter();  

            services.AddCybtansServices(typeof(@{SERVICE}Stub).Assembly);
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var asyncQueryExecutioner = new EfAsyncQueryExecutioner();
            asyncQueryExecutioner.SetCurrent();

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
                
                c.OAuthClientId("swagger");
                c.OAuthClientSecret(Configuration.GetValue<string>("Identity:Secret"));
                c.OAuthAppName("@{SERVICE}");
                c.OAuthUsePkce();
            });
            
             app.UseReDoc(c =>
            {
                c.RoutePrefix = "docs";
                c.SpecUrl("/swagger/v1/swagger.json");
                c.DocumentTitle = "@{SERVICE} API";
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

        void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "@{SERVICE}", Version = "v1" });
                c.OperationFilter<SwachBuckleOperationFilters>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });


                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{Configuration.GetValue<string>("Identity:Swagger")}/connect/authorize"),
                            TokenUrl = new Uri($"{Configuration.GetValue<string>("Identity:Swagger")}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "api", "@{SERVICE} API" }
                            }
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                             Reference = new OpenApiReference
                             {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "oauth2"
                             }
                        },
                        new string[0]
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[0]
                    }
                });
            });
        }

        void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = Configuration.GetValue<string>("Identity:Authority");
                options.Audience = $"{options.Authority}/resources";                 
                options.RequireHttpsMetadata = false;                    
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = options.Audience,
                    ValidIssuer = options.Authority,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Identity:Secret")))
                };
                options.Validate();
            });

            services.AddAuthorization();
		}
    }
}
