using OpenQA.Selenium;
using System;
using System.Collections;

namespace Cybtans.Automation
{
    public class WebElementCollectionSyncHandler : PropertySyncHandler
    {
        public override bool Sync(IWebDriver driver, ISearchContext searchContext)
        {
            var elements = GetWebElements(driver, searchContext);
            SetValue((IList)Activator.CreateInstance(Property.PropertyType, elements));

            return elements != null;
        }
    }

}
