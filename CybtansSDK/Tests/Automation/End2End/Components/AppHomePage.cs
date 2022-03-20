using Cybtans.Automation;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace End2End.Components
{
    public class AppHomePage : PageBase
    {     

        [TestElement("#root > header", IsRequired = true)]
        public NavBarComponent Header { get; set; }

        [TestElement("#root > .container", IsRequired = true)]
        public HomeContentComponent Content { get; set; }
    }

    public class NavBarComponent : TestComponent
    {
        [TestElement(".navbar-brand")]
        private IWebElement Brand { get; set; }

        [TestElement(".nav-item .nav-link")]
        public List<IWebElement> Links { get; set; }

        public string BrandText => Brand?.Text;
    }

    public class HomeContentComponent : TestComponent
    {
        [TestElement("//div/h1", Type = SelectorType.XPath)]
        public IWebElement HellowMessage { get; set; }

        [TestElement("ul:nth-child(3)")]
        public SectionList Links { get; set; }

        [TestElement("ul:nth-child(5)")]
        public SectionList Setup { get; set; }
    }


    public class SectionList : TestComponent
    {
        [TestElement("li")]
        public List<IWebElement> Items { get; set; }
    }
}
