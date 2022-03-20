using Cybtans.Automation.Contexts;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Automation.Xunit
{
    public class AutomationFixture : IAsyncLifetime
    {
        object @lock = new();

        public static IConfiguration Configuration { get; private set; }

        public AutomationFixture()
        {
            lock (@lock)
            {
                if (Configuration == null)
                {
                    Configuration = CreateConfiguration();
                }
            }

            DriverType = Configuration["Driver"] ?? SupportedDrivers.DRIVER_CHROME;
            Context = new WebDriverContext(DriverType);
        }
        
        public WebDriverContext Context { get; }

        public string DriverType { get; set; }

        public bool AutoInitialize { get; set; } = true;
        
        public virtual Task DisposeAsync()
        {
            Context.Dispose();
            return Task.CompletedTask;
        }

        public virtual async Task InitializeAsync()
        {
            if (AutoInitialize)
            {
                await Context.InitializeAsync(GetType());
            }
        }

        public static IConfigurationRoot CreateConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true);

            return configBuilder.Build();
        }
    }

    public class AutomationFixture<TPage> : AutomationFixture
        where TPage : IPage
    {
        public AutomationFixture()
        {
          
        }
        
        public TPage? Page { get; private set; }

        public string? Url { get; set; }

        public override async Task InitializeAsync()
        {            
            await base.InitializeAsync();

            var attr = GetType().GetCustomAttribute<PageUrlAttribute>();
            if (attr != null)
            {
                Url = attr.Url;
                if (attr.IsRelative)
                {
                    if (!string.IsNullOrEmpty(Url) && !Url.StartsWith("/"))
                        Url = "/" + Url;

                    Url = Configuration["Url"] + Url;
                }
            }

            Page = PageBase.Create<TPage>(Context.Driver, Url);

            if (Url != null)
            {
                await Page.LoadAsync();
            }

            var useDriverAttr = GetType().GetCustomAttribute<UseDriverAttribute>();
            if(useDriverAttr != null && useDriverAttr.DisableNavigation)
            {
                await Page.SyncAsync(true);
            }
        }
    }
}
