using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Microsoft.Extensions.Hosting;

namespace Cybtans.AspNetCore.Migrations
{
    public static class WebHostExtensions
    {
        public static bool IsInKubernetes(this IHost host)
        {            
            var cfg = host.Services.GetService<IConfiguration>();
            var orchestratorType = cfg["OrchestratorType"];
            return orchestratorType?.ToUpper() == "K8S";
        }

        public static IHost MigrateDbContext<TContext, TSeed>(this IHost host)
            where TContext :DbContext
            where TSeed : IDbContextSeed<TContext>
        {
            return MigrateDbContext<TContext>(host, (dbContext, provider) =>
            {
                var seed = provider.GetRequiredService<TSeed>();
                seed.Seed(dbContext).Wait();
            });
        }

        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder = null)
            where TContext : DbContext
        {
            var underK8s = host.IsInKubernetes();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    if (underK8s)
                    {
                        InvokeSeeder(seeder, context, services, true);
                    }
                    else
                    {
                        var retry = Policy.Handle<Exception>()
                             .WaitAndRetry(new TimeSpan[]
                             {
                             TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(5),
                             TimeSpan.FromSeconds(8),
                             });

                        //if the sql server container is not created on run docker compose this
                        //migration can't fail for network related exception. The retry options for DbContext only 
                        //apply to transient exceptions
                        // Note that this is NOT applied when running some orchestrators (let the orchestrator to recreate the failing service)
                        retry.Execute(() => InvokeSeeder(seeder, context, services, true));
                    }

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                    if (underK8s)
                    {
                        throw;          // Rethrow under k8s because we rely on k8s to re-run the pod
                    }
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services, bool migrate)
            where TContext : DbContext
        {
            if (migrate)
            {
                context.Database.Migrate();
            }
            seeder?.Invoke(context, services);
        }

        public static IHost Seed<TContext, TSeed>(this IHost host)
         where TContext : DbContext
         where TSeed : IDbContextSeed<TContext>
        {
            return Seed<TContext>(host, (dbContext, provider) =>
            {
                var seed = provider.GetRequiredService<TSeed>();
                seed.Seed(dbContext).Wait();
            });
        }

        public static IHost Seed<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            var underK8s = host.IsInKubernetes();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    if (underK8s)
                    {
                        InvokeSeeder(seeder, context, services, false);
                    }
                    else
                    {
                        var retry = Policy.Handle<Exception>()
                             .WaitAndRetry(new TimeSpan[]
                             {
                             TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(5),
                             TimeSpan.FromSeconds(8),
                             });

                        //if the sql server container is not created on run docker compose this
                        //migration can't fail for network related exception. The retry options for DbContext only 
                        //apply to transient exceptions
                        // Note that this is NOT applied when running some orchestrators (let the orchestrator to recreate the failing service)
                        retry.Execute(() => InvokeSeeder(seeder, context, services, false));
                    }

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                    if (underK8s)
                    {
                        throw;          // Rethrow under k8s because we rely on k8s to re-run the pod
                    }
                }
            }

            return host;
        }
    }
}
