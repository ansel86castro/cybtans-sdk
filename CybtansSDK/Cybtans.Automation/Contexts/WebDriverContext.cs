using OpenQA.Selenium;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

#nullable enable

namespace Cybtans.Automation.Contexts
{
    public class WebDriverContext : IWebDriverContext, IDisposable
    {
        public enum ContextStatus
        {
            None = 0,
            Initialized,
            Initializing,
            Disposed
        }

        IWebDriver? _webDriver;
        string? _driverName;
        bool _useDriverBarrier;
        WaitHandleBarrier? _barrier;
        bool _navigate = true;
        object @lock = new object();
        ContextStatus _status;
        AutoResetEvent _initializeEvent;
        string _driverType;

        public WebDriverContext(string driverType = SupportedDrivers.DRIVER_CHROME)
        {
            _driverType = driverType;
            _initializeEvent = new AutoResetEvent(false);
        }

        public bool Navigate => _navigate;

        public IWebDriver? Driver => _webDriver;

        public ContextStatus Status => _status;

        public string? DriverName => _driverName;

        public virtual async Task InitializeAsync(Type? contextType = null)
        {
            contextType ??= GetType();

            Task? waitForInitialization = null;
            lock (@lock)
            {
                if (_status == ContextStatus.Initialized)
                    return;

                else if (_status == ContextStatus.Initializing)
                {
                    waitForInitialization = _initializeEvent.ToTask((int)BarrierManager.WaitHandleTimeout.TotalMilliseconds);
                }
                else
                {
                    _status = ContextStatus.Initializing;
                }
            }

            if (waitForInitialization != null)
            {
                await waitForInitialization;
                return;
            }

            DriverContextAttribute? useDriverAttr = contextType.GetCustomAttribute<DriverContextAttribute>(true);
            if (useDriverAttr != null)
            {
                _driverName = useDriverAttr.DriverName;
                _navigate = !useDriverAttr.DisableNavigation;
                if (useDriverAttr.WaitForBarrier)
                {
                    _webDriver = await WebDriverManager.Instance.WaitForDriver(_driverName);
                }
                else
                {
                    int references = useDriverAttr.MaxReference <= 0 ? GetDriverReferences(_driverName, contextType) : useDriverAttr.MaxReference;
                    _webDriver = WebDriverManager.Instance.CreateDriver(_driverType, _driverName, references);
                    _useDriverBarrier = true;
                    _webDriver.Manage().Window.Maximize();
                }
            }
            else
            {
                _webDriver = WebDriverManager.CreateDriver(_driverType);
                _webDriver.Manage().Window.Maximize();
            }

            var barrierDefAttr = contextType.GetCustomAttribute<BarrierAttribute>();
            if (barrierDefAttr != null)
            {
                _barrier = BarrierManager.Manager.GetWaitHandler(barrierDefAttr.BarrierName ?? contextType.Name);
            }

            var barriersAttr = contextType.GetCustomAttributes<WaitForBarrierAttribute>();
            if (barriersAttr != null && barriersAttr.Any())
            {
                await Task.WhenAll(barriersAttr.Select(x => BarrierManager.Manager.Wait(x.BarrierName)));
            }

            _status = ContextStatus.Initialized;
            _initializeEvent.Set();
        }

        public async Task<TPage> CreatePageAsync<TPage>(string? baseUrl = null) where TPage : IPage
        {
            if (_webDriver == null)
            {
                _webDriver = WebDriverManager.CreateDriver(_driverType);
                _webDriver.Manage().Window.Maximize();
            }

            var page = PageBase.Create<TPage>(_webDriver, baseUrl);
            await page.LoadAsync();
            return page;
        }

        public TPage CreatePage<TPage>(string? baseUrl = null) where TPage : IPage
        {
            if (_webDriver == null)
            {
                _webDriver = WebDriverManager.CreateDriver(_driverType);
                _webDriver.Manage().Window.Maximize();
            }

            var page = PageBase.Create<TPage>(_webDriver, baseUrl);
            page.Load();
            return page;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_status == ContextStatus.Disposed)
                return;

            if (disposing)
            {
                if (_driverName != null)
                {
                    if (_useDriverBarrier)
                    {
                        WebDriverManager.Instance.ReleaseBarrier(_driverName);
                    }

                    WebDriverManager.Instance.ReleaseReference(_driverName);
                }
                else
                {
                    _webDriver?.Quit();
                }

                _barrier?.Set();
            }
            _status = ContextStatus.Disposed;
        }

        private int GetDriverReferences(string driverName, Type testType)
        {
            int count = 1;
            foreach (var type in testType.Assembly.ExportedTypes)
            {
                var useDriverAttr = type.GetCustomAttribute<UseDriverAttribute>();
                if (useDriverAttr != null && useDriverAttr.DriverName == driverName)
                {
                    count++;
                }
            }

            return count++;
        }

      
    }

}
