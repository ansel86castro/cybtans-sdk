using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cybtans.Automation
{
    public class WebDriverManager: BarrierManager
    {    
        private class WebDriverCacheItem
        {
            public IWebDriver Driver;
            public int ReferenceCount;            
        }

        static WebDriverManager()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, arg) =>
              {
                  Instance.Dispose();
                  BarrierManager.Manager.Dispose();
              };
        }

        private Dictionary<string, WebDriverCacheItem> _cache = new Dictionary<string, WebDriverCacheItem>();        

        public static WebDriverManager Instance { get; } = new WebDriverManager();

        public IWebDriver CreateDriver(string driverType, string driverName, int maxReferenceCount)
        {
            CheckDisposed();

            lock (_cache)
            {
                if (!_cache.TryGetValue(driverName, out var item))
                {
                    item = new WebDriverCacheItem
                    {
                        Driver = CreateDriver(driverType),
                        ReferenceCount = maxReferenceCount
                    };

                    _cache.Add(driverName, item);
                }
               
                return item.Driver;
            }
        }

        public bool ContainsDriver(string driverName)
        {
            return _cache.ContainsKey(driverName);
        }

        public IWebDriver GetDriver(string driverName)
        {
            lock (_cache)
            {
                if(!_cache.TryGetValue(driverName, out var driver))
                {
                    new InvalidOperationException($"Driver for {driverName} not found");
                }
                return _cache[driverName].Driver;
            }
        }

        public static IWebDriver CreateDriver(string driver)
        {            
            switch (driver)
            {
                case SupportedDrivers.DRIVER_CHROME:
                    return new ChromeDriver();
                //case SupportedDrivers.DRIVER_FIREFOX:
                //    return new FirefoxDriver();
                //case SupportedDrivers.DRIVER_IE:
                //    return new InternetExplorerDriver();
                //case SupportedDrivers.DRIVER_Edge:
                //    return new EdgeDriver();
                case SupportedDrivers.DRIVER_CHROME_HEADLESS:
                    {
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.AddArguments("headless");

                        return new ChromeDriver(chromeOptions);
                    }
                default:
                    throw new InvalidOperationException("Driver not found");
            }
        }

        public async Task<IWebDriver> WaitForDriver(string driverName)
        {
            CheckDisposed();

            await GetWaitHandler(driverName).CreateTask();

            return GetDriver(driverName);          
        }

        public void ReleaseReference(string driverName)
        {
            CheckDisposed();

            lock (_cache)
            {
                _cache.TryGetValue(driverName, out var item);
                item.ReferenceCount--;
                if (item.ReferenceCount <= 0)
                {
                    item.Driver.Quit();
                    _cache.Remove(driverName);
                    RemoveHandler(driverName);
                }
            }           
        }

        public void ReleaseBarrier(string barrier)
        {
            CheckDisposed();

            GetWaitHandler(barrier).Set();
        }

        #region IDisposable Support      

        //~WebDriverManager()
        //{
        //    try
        //    {
        //        Dispose(false);
        //    }
        //    catch
        //    {

        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {               
                foreach (var item in _cache.Values)
                {
                    item.Driver.Quit();
                }
                
                _cache = null;
            }

            base.Dispose(disposing);
        }
                             
        #endregion
    }
}
