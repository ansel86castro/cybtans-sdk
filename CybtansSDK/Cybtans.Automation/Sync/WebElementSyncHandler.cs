using OpenQA.Selenium;

namespace Cybtans.Automation
{
    public class WebElementSyncHandler : PropertySyncHandler
    {
        public override bool Sync(IWebDriver driver, ISearchContext searchContext)
        {
            var element = GetWebElement(driver, searchContext);
            SetValue(element);

            return element != null;
        }
    }

}
