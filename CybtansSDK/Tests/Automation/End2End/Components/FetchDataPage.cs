using OpenQA.Selenium;
using Cybtans.Automation;
using Cybtans.Automation.Components;

namespace End2End.Components
{
    public class FetchDataPage : PageBase
    {    

        [TestElement("#tabelLabel", IsRequired = true)]
        public IWebElement Title { get; set; }

        [TestElement("#tabelLabel + p", IsRequired =true)]
        public IWebElement Description { get; set; }

        [TestElement("table")]
        public BasicTable Table { get; set; }

        [TestElement(IsRequired = true)]
        public PaginationComponent Pagination { get; set; }

        public bool IsLoading => Pagination?.LoadingIndicator != null;

        public void LoadNext() 
        {
            Pagination?.Click(x => x.NextPage);
            WaitForLoadCompleted();
        }

        public void LoadPrevious()
        {
            Pagination?.Click(x => x.PreviousPage);
            WaitForLoadCompleted();
        }

        public void WaitForLoadCompleted()
        {
            this.Sync(x => x.Pagination).WaitUntil(x => x.Pagination.LoadingIndicator == null);
        }
    }
  
    public class PaginationComponent : TestComponent
    {
        [TestElement("table + div > a:first-child")]
        public IWebElement PreviousPage { get; set; }

        [TestElement("table + div > a:last-child")]
        public IWebElement NextPage { get; set; }

        [TestElement("table + div > span")]
        public IWebElement LoadingIndicator { get; set; }
    }

}
