using Cybtans.Automation;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Cybtans.Automation
{
    public abstract class PropertySyncHandler
    {
        public TestComponent Target { get; set; }

        public By Selector { get; set; }

        public bool IsRequired { get; set; }

        public PropertyInfo Property { get; set; }

        public bool IsComposed { get; set; }

        public object Value => Property != null && Target != null ? Property.GetValue(Target) : null;

        protected void SetValue(object value)
        {
            Property.SetValue(Target, value);
        }

        public abstract bool Sync(IWebDriver driver, ISearchContext searchContext);

        protected IWebElement GetWebElement(IWebDriver driver, ISearchContext searchContext)
        {
            if (Selector == null)
                return null;

            try
            {
                return Selector.FindElement(searchContext);
            }
            catch (NoSuchElementException)
            {
                if (IsRequired)
                {
                    SetValue(null);
                    throw;
                }

                return null;
            }
            catch (StaleElementReferenceException e)
            {
                throw;
            }
        }

        protected ReadOnlyCollection<IWebElement> GetWebElements(IWebDriver driver, ISearchContext searchContext)
        {
            if (Selector == null)
                return null;

            try
            {
                return searchContext.FindElements(Selector);
            }
            catch (NoSuchElementException)
            {
                if (IsRequired)
                {
                    SetValue(null);
                    throw;
                }
                return null;
            }
            catch (StaleElementReferenceException e)
            {
                throw;
            }

        }
    }

}
