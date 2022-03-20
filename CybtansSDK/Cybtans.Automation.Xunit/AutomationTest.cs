using Cybtans.Automation.Contexts;
using Cybtans.Testing;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

#nullable enable

namespace Cybtans.Automation.Xunit
{
    public class AutomationTest<TPage, TFixture>: OrderedTest, IClassFixture<TFixture> ,IAsyncLifetime
        where TPage : IPage
        where TFixture :AutomationFixture<TPage>
    {        
        public AutomationTest(TFixture fixture, string? url = null)
        {
            Context = fixture.Context;
            Fixture = fixture;
            Url = url;

          
        }

        public WebDriverContext Context { get; }
        public string? Url { get; }
        public TFixture Fixture { get; }   
        
        public TPage? Page { get; private set; }        

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {                    
            if (Context.Status != WebDriverContext.ContextStatus.Initialized)
            {
                await Context.InitializeAsync(GetType());
            }

            Page = Fixture.Page;

            if (Page != null)
            {
                var autoSync = GetType().GetCustomAttribute<AutoSyncAttribute>();
                if (autoSync != null)
                {
                    await Page.SyncAsync(autoSync.WaitForElements);
                }
                else
                {
                    await Page.SyncAsync();
                }
            }
        }
    }

    public class AutomationTest<TPage> : AutomationTest<TPage, AutomationFixture<TPage>>
        where TPage : IPage
    {
        public AutomationTest(AutomationFixture<TPage> fixture, string? url = null) : base(fixture, url)
        {
        }
    }
}
