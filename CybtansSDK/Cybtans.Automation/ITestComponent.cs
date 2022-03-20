using OpenQA.Selenium;
using System.Threading.Tasks;

namespace Cybtans.Automation
{
    public interface ITestComponent
    {
        IWebDriver Driver { get; }

        IWebElement Container { get; }

        void Init(IWebDriver driver, IWebElement container);

        bool Sync(bool waitForElements = false);

        public Task<bool> SyncAsync(bool waitForElements = false)
        {
            return Task.Run(() => Sync(waitForElements));
        }
    }

}
