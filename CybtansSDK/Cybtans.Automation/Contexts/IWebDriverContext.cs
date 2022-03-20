using OpenQA.Selenium;

#nullable enable

namespace Cybtans.Automation.Contexts
{
    public interface IWebDriverContext
    {
        IWebDriver? Driver { get; }
    }

}
