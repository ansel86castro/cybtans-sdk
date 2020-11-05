using Cybtans.Automation;
using OpenQA.Selenium;
using System;

namespace Cybtans.Automation
{
    public class TestComponentSyncHandler : PropertySyncHandler
    {
        TestComponent component;
        public TestComponentSyncHandler()
        {
            IsComposed = true;
        }

        public override bool Sync(IWebDriver driver, ISearchContext searchContext)
        {
            var container = GetWebElement(driver, searchContext);

            if (Selector != null && container == null)
            {
                component = null;
                SetValue(null);
                return false;
            }

            if (component == null)
            {
                component = (TestComponent)Activator.CreateInstance(Property.PropertyType);
            }

            component.Parent = Target;
            component.Page = Target as IPage ?? Target.Page;

            component.Init(driver, container);
            SetValue(component);
            return component.Sync(driver);
        }
    }

}
