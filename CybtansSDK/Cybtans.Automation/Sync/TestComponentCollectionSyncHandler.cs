using Cybtans.Automation;
using OpenQA.Selenium;
using System;
using System.Collections;

namespace Cybtans.Automation
{

    public class TestComponentCollectionSyncHandler : PropertySyncHandler
    {
        public TestComponentCollectionSyncHandler()
        {
            IsComposed = true;
        }

        public Type ComponentType { get; set; }

        public override bool Sync(IWebDriver driver, ISearchContext searchContext)
        {
            if (Selector == null)
                throw new InvalidOperationException($"Selector required for {Property.Name}");

            var list = (IList)Activator.CreateInstance(Property.PropertyType);
            var elements = GetWebElements(driver, searchContext);
            if (elements == null)
            {
                SetValue(null);
                return false;
            }

            var sync = true;
            foreach (var element in elements)
            {
                var component = (TestComponent)Activator.CreateInstance(ComponentType);
                component.Parent = Target;
                component.Page = Target as IPage ?? Target.Page;

                component.Init(driver, element);

                var componentSync = component.Sync(driver);
                if (!componentSync)
                {
                    sync = false;
                }

                list.Add(component);
            }

            SetValue(list);

            return sync;
        }
    }

}
