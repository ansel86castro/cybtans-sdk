using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Cybtans.AspNetCore;
using Cybtans.Entities.EntityFrameworkCore;
using Cybtans.Services.Extensions;
using Cybtans.Tests.Services;
using System.Data.Common;
using Cybtans.Tests.Domain.EF;
using Cybtans.Tests.Domain;
using Cybtans.Messaging;
using Cybtans.Tests.Grpc;
using Cybtans.AspNetCore.Interceptors;
using Cybtans.Tests.RestApi;
using GraphQL.Types;
using Cybtans.Tests.GraphQL;
using Cybtans.Tests.RestApi.Security;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using System.Security.Cryptography;
using System.IO;
using GraphQL;
using Cybtans.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cybtans.Test.RestApi
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
                         .AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod();
                    });
            });

            #endregion

            #region Data Access Layer

            services.AddSingleton(AdventureContext.CreateInMemoryDatabase("RestAPI"));
            services.AddDbContext<AdventureContext>(
                (srv,builder) =>
                {
                    var conn = srv.GetRequiredService<DbConnection>();
                    builder.UseSqlite(conn);
                });

            services
            .AddUnitOfWork<AdventureContext>()
            .AddRepositories();

            services.AddTransient<AdventureContextSeed>();
            services.AddDatabaseConnectionFactory(o => o.ConnectionFactoryDelegate = () => AdventureContext.CreateInMemoryDatabase("RestAPI"));

            #endregion

            #region WebAPI
            
            services
            .AddControllers(options =>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();
            })
            //.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining(typeof(TestStub)))
            .AddCybtansFormatter();
            services.TryAddSingleton<IHttpContentSerializer, CybtansContentSerializer>();
            #endregion

            #region App Services

            services.AddOptions<JwtOptions>().Bind(Configuration.GetSection("Jwt"));
            services.AddAutoMapper(typeof(TestStub));
            services.AddCybtansServices(typeof(TestStub).Assembly);

            services.AddSingleton<EntityEventDelegateHandler<OrderMessageHandler>>();
            services.AddTransient<OrderMessageHandler>();
            services.AddMessageHandler<CustomerEvent, Customer>();
            services.AddDefaultValidatorProvider(p =>
            {
                p.AddValidatorFromAssembly(typeof(TestStub).Assembly);
            });
            services.AddSingleton<IMessageInterceptor, ValidatorActionInterceptor>();

            #endregion

            #region Messaging

            services.AddRabbitMessageQueue(Configuration)
             .ConfigureSubscriptions(sm =>
             {
                 sm.SubscribeHandlerForEvents<Order, OrderMessageHandler>("Test");
                 sm.SubscribeForEvents<CustomerEvent, Customer>("Test");

             });

            services.AddDbContextEventPublisher<AdventureContext>();

            services.AddAccessTokenManager(Configuration);

            services.AddRabbitBroadCastService(Configuration.GetSection("BroadCastOptions").Get<BroadcastServiceOptions>());
           
            #endregion

            #region Caching 

            services.AddLocalCache();

            #endregion

            #region Grpc Clients

            //Add Grpc clients
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            services.AddGrpcClient<Greeter.GreeterClient>(o =>
            {
                o.Address = new Uri(Configuration["GreteerService"]);
            });

            #endregion

            #region GraphQL
        
            services.AddSingleton<ISchema, TestQueryDefinitionsSchema>();
            services.AddGraphQL(b =>
            {
                b.AddSystemTextJson();
                b.AddGraphTypes();                
            });
             

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            EfAsyncQueryExecutioner.Setup();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandlingMiddleware();
            }

            app.UseCors();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cybtans.Test API V1");
                c.EnableFilter();
                c.EnableDeepLinking();
                c.ShowCommonExtensions();  
                
                c.OAuthClientId("swagger");
                c.OAuthClientSecret(Configuration.GetValue<string>("Identity:Secret"));
                c.OAuthAppName("Cybtans.Test");
                c.OAuthUsePkce();
            });
            
             app.UseReDoc(c =>
            {
                c.RoutePrefix = "docs";
                c.SpecUrl("/swagger/v1/swagger.json");
                c.DocumentTitle = "Cybtans.Test API";
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
            });
        }

        void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cybtans.Test", Version = "v1" });
                c.OperationFilter<SwachBuckleOperationFilters>();

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
                        Log.Logger.Information($"Token Validated");
                    },
                    OnAuthenticationFailed = async cts =>
                    {
                        Log.Logger.Information($"Autnentication Failed");
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
                    IssuerSigningKey = AuthenticationService.GetPublicRsaSecurityKey(),//  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:Secret"))),
                    ClockSkew = TimeSpan.Zero
                };
                options.Validate();
            });
           
            services.AddAuthorization(options=>
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

      
        public static void GenerateKeys()
        {
            if (!File.Exists("keys/public.key"))
            {
                if (!Directory.Exists("keys"))
                {
                    Directory.CreateDirectory("keys");
                }


                using (var rsa = RSA.Create())
                {                                 
                    File.WriteAllText("keys/public.key", rsa.ExportRSAPublicKeyPem());

                    var xml = rsa.ToXmlString(true);
                    File.WriteAllText("keys/private.key", xml);
                }
            }
        }
    }
}
