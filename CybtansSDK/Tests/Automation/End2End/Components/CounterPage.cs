using OpenQA.Selenium;
using Cybtans.Automation;

namespace End2End.Components
{
    public  class CounterPage:PageBase
    {       
        [TestElement("#root > .container > p > strong")]
        public IWebElement CounterValue { get; set; }        

        [TestElement("#root > .container > button")]
        public IWebElement IncButton { get; set; }
    }
}
