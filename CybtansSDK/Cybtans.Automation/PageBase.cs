using OpenQA.Selenium;
using System;
using System.Threading.Tasks;

namespace Cybtans.Automation
{
#nullable enable

    public interface IPage : ITestComponent
    {
        string? BaseUrl { get; set; }

        string? RelativeUrl { get; }

        void Load();

        public Task LoadAsync()
        {
            return Task.Run(() => Load());
        }
    }

    public abstract class PageBase : TestComponent, IPage
    {
        public string? BaseUrl { get; set; }

        public virtual string? RelativeUrl { get; protected set; }

        public virtual void Load()
        {
            if (BaseUrl != null)
            {
                Driver.Navigate().GoToUrl(BaseUrl + (RelativeUrl != null ? "/" + RelativeUrl : ""));
            }

            OnLoaded();

            Sync(true);
        }

        public virtual void OnLoaded() { }

        public static TPage Create<TPage>(IWebDriver driver, string? baseUrl) where TPage : IPage
        {
            var page = (TPage)Activator.CreateInstance(typeof(TPage))!;
            page.BaseUrl = baseUrl;
            page.Init(driver, null);
            return page;
        }

        public static TPage CreateAndLoad<TPage>(IWebDriver driver, string? baseUrl = null, string? relativeUrl = null)
            where TPage : PageBase
        {
            var page = Create<TPage>(driver, baseUrl);
            page.RelativeUrl = relativeUrl;
            page.Load();
            return page;
        }

        public static TPage CreateAndSync<TPage>(IWebDriver driver, string? baseUrl = null)
           where TPage : PageBase
        {
            var page = Create<TPage>(driver, baseUrl);
            page.Sync(true);
            return page;
        }

        /// <summary>
        /// Executes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="javascript"></param>
        /// <remarks>
        /// Executes JavaScript in the context of the currently selected frame or window.
        /// This means that "document" will refer to the current document. If the script
        /// has a return value, then the following steps will be taken:
        /// • For an HTML element, this method returns a OpenQA.Selenium.IWebElement
        /// • For a number, a System.Int64 is returned
        /// • For a boolean, a System.Boolean is returned
        /// • For all other cases a System.String is returned.
        /// • For an array,we check the first element, and attempt to return a System.Collections.Generic.List`1
        ///   of that type, following the rules above. Nested lists are not supported.
        /// • If the value is null or there is no return value, null is returned.
        /// </remarks>
        /// <returns></returns>
        public object ExecuteScript(string javascript)
        {
            var executor = (IJavaScriptExecutor)Driver;
            return executor.ExecuteScript(javascript);
        }
    }

#nullable restore
}
