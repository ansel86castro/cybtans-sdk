using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cybtans.AspNetCore;
using Cybtans.Clients;
using Cybtans.Messaging;
using Cybtans.Services.Extensions;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Gateway.GraphQL;
using Cybtans.Tests.Gateway.Security;
using Cybtans.Tests.Grpc;
using Cybtans.Tests.Models;
using Cybtans.Tests.Services;
using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SignalRChat.Hubs;

namespace Cybtans.Tests.Gateway
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

            AddSwagger(services);
            AddAuthentication(services);
         
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                         .WithOrigins("http://localhost:3000", "https://localhost:6001")
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .WithExposedHeaders("Content-Type", "Content-Disposition")
                         .AllowCredentials();
                    });
            });

            //Add Rest service Clients
            RegisterClients(services);

            services
                .AddControllers(options => options.Filters.Add(new UpstreamExceptionFilter()))
                .AddCybtansFormatter();

            #region Messaging

            services.AddScoped<OrderNotificationHandler>();
            services.AddRabbitBroadCastService(Configuration.GetSection("BroadCastOptions").Get<BroadcastServiceOptions>(), sm =>
            {
                sm.Subscribe<OrderNotification, OrderNotificationHandler>("Orders");
            });

            #endregion

            //Add Signal R
            services.AddSignalR();

            //Add Grpc clients
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            services.AddGrpcClient<Greeter.GreeterClient>(o =>
            {                
                o.Address = new Uri(Configuration["GreteerService"]);
            });

            services.AddCybtansServices(typeof(Startup).Assembly);

            #region GraphQL

            services.AddSingleton<ISchema, ApiGatewayDefinitionsSchema>();
            services.AddGraphQL(b=>
                b.AddSystemTextJson()
                .AddGraphTypes());

            #endregion

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cybtans.Test Gateway V1");
                c.EnableFilter();
                c.EnableDeepLinking();
                c.ShowCommonExtensions();
            });

            app.UseReDoc(c =>
            {
                c.RoutePrefix = "docs";
                c.SpecUrl("/swagger/v1/swagger.json");
                c.DocumentTitle = "Cybtans.Test API";                
            });


            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            #region GraphQL

            // add http for Schema at default url /graphql
            app.UseGraphQL<ISchema>();

            // use graphql-playground at default url /ui/playground
            app.UseGraphQLPlayground("/ui/playground");

            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<OrderNotificationHub>("/ordershub");
            });
        }


        void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cybtans.Test", Version = "v1" });
                c.OperationFilter<SwachBuckleOperationFilters>();
                c.SchemaFilter<SwachBuckleSchemaFilters>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {                   
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

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Cybtans.Tests.Models.xml"));
            });
        }

        void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnTokenValidated = async ctx =>
                    {

                    },
                    OnAuthenticationFailed = async cts =>
                    {

                    },
                    OnMessageReceived = async ctx =>
                    {

                    },
                    OnForbidden = async ctx =>
                    {

                    },
                    OnChallenge = async ctx =>
                    {

                    }
                };
             
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = Configuration.GetValue<string>("Jwt:Audience"),
                    ValidIssuer = Configuration.GetValue<string>("Jwt:Issuer"),
                    IssuerSigningKey = AuthenticationService.GetPublicRsaSecurityKey(), //new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:Secret"))),                    
                    ClockSkew = TimeSpan.Zero
                };
                options.Validate();
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminUser", policy => policy.RequireRole("admin"));

                options.AddPolicy("ClientPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new ClientPolicyRequirement());

                });

                options.AddPolicy("ClientCreator", policy =>
                {
                    policy.AddRequirements(new ClientCreatorRequiriment());
                });
            });

            services.AddSingleton<IAuthorizationHandler, ClientPolicyHandlers>();
            services.AddSingleton<IAuthorizationHandler, ClientCreatorPolicyHandler>();
        }


        public void RegisterClients(IServiceCollection services)
        {
            services.AddAuthenticatedHttpHandler();

            services.AddClients(typeof(ClientServiceClient).Assembly)
                    .ConfigureTransport(Configuration.GetValue<string>("Tests"))
                    .AddHttpMessageHandler<HttpClientAuthorizationHandler>()
                    .AddHttpMessageHandler(() => new HttpVersionRequestHandler(HttpVersion.Version20));
            
            //static void BuilderConfig(IHttpClientBuilder builder, Type type) => builder.AddHttpMessageHandler<HttpClientAuthorizationHandler>();
            //services.AddClients(Configuration.GetValue<string>("Tests"), typeof(IClientServiceClient).Assembly, BuilderConfig);            
        }
    }
}
