using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Cybtans.Automation;

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

    public class TestComponent : ITestComponent
    {
        public static TimeSpan WaitSpan = TimeSpan.FromSeconds(5);

        private IWebDriver _webDriver;
        private IWebElement _container;
        internal Dictionary<string, PropertySyncHandler> _syncHandler = new Dictionary<string, PropertySyncHandler>();

        public TestComponent(IWebDriver webDriver, IWebElement container)
        {
            _webDriver = webDriver;
            _container = container;
        }

        public TestComponent(IWebDriver webDriver) : this(webDriver, null) { }

        public TestComponent() { }

        public TestComponent Parent { get; internal set; }

        public IPage Page { get; internal set; }

        public bool Sync(bool waitForElements = false)
        {
            if (waitForElements)
            {
                return Wait().Until(driver =>
                {
                    try
                    {
                        Sync(driver);
                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
            }
            return Sync(Driver);
        }

        public virtual void Init(IWebDriver driver, IWebElement container)
        {
            _webDriver = driver;
            _container = container;

            if (_syncHandler.Count == 0)
            {
                InitializeSync();
            }
        }

        internal ISearchContext SearchContext => (ISearchContext)_container ?? _webDriver;

        public IWebDriver Driver { get => _webDriver; }

        public IWebElement Container { get => _container; }

        internal WebDriverWait Wait()
        {
            return new WebDriverWait(_webDriver, WaitSpan);
        }

        internal protected virtual bool Sync(IWebDriver webDriver)
        {
            var result = true;

            try
            {
                foreach (var (_, handler) in _syncHandler)
                {
                    bool sync = handler.Sync(webDriver, SearchContext);

                    if (!sync)
                    {
                        result = false;
                    }
                }

                OnSyncCompleted(result);
            }
            catch (NoSuchElementException)
            {
                OnSyncCompleted(false);
                throw;
            }

            return result;
        }

        private void InitializeSync()
        {
            var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var p in props)
            {
                TestElementAttribute attr = p.GetCustomAttribute<TestElementAttribute>();
                if (attr == null)
                    continue;

                PropertySyncHandler handler = null;

                if (typeof(IWebElement).IsAssignableFrom(p.PropertyType))
                {
                    handler = new WebElementSyncHandler();
                }
                else if (typeof(TestComponent).IsAssignableFrom(p.PropertyType))
                {
                    handler = new TestComponentSyncHandler();
                }
                else if (typeof(IList).IsAssignableFrom(p.PropertyType))
                {
                    if (!p.PropertyType.IsGenericType)
                        throw new InvalidOperationException($"Unable to find component type for {p.DeclaringType.Name}.{p.Name}");

                    var genericArgs = p.PropertyType.GetGenericArguments();
                    if (genericArgs.Length > 1)
                        throw new InvalidOperationException($"Invalid Collection for {p.DeclaringType.Name}.{p.Name} it must have only one generic argument");

                    if (typeof(IWebElement).IsAssignableFrom(genericArgs[0]))
                    {
                        handler = new WebElementCollectionSyncHandler();
                    }
                    else if (typeof(TestComponent).IsAssignableFrom(genericArgs[0]))
                    {
                        handler = new TestComponentCollectionSyncHandler() { ComponentType = genericArgs[0] };
                    }

                }

                if (handler != null)
                {
                    handler.IsRequired = attr.IsRequired;
                    handler.Property = p;
                    handler.Selector = attr.GetSelector();
                    handler.Target = this;
                    _syncHandler.Add(p.Name, handler);
                }

            }
        }

        protected virtual void OnSyncCompleted(bool syncResult)
        {

        }

        #region Helpers

        public static Func<IWebDriver, IWebElement> ElementExists(By by)
        {
            return driver =>
            {
                try
                {
                    return driver.FindElement(by);
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };
        }

        public static Func<IWebDriver, ReadOnlyCollection<IWebElement>> ElementsExists(By by)
        {
            return driver =>
            {
                try
                {
                    return driver.FindElements(by);
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };
        }

        public static T CreateComponent<T>(IWebDriver driver, IWebElement container = null) where T : ITestComponent
        {
            var component = Activator.CreateInstance<T>();

            component.Init(driver, container);
            component.Sync(true);

            return component;
        }
        #endregion

    }

}
