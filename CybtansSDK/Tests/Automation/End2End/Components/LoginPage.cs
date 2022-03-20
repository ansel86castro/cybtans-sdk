using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Cybtans.Automation;

namespace End2End.Components
{
   public class LoginPage:PageBase
    {     

        [TestElement("input[name=\"username\"]")]
        public IWebElement Username { get; set; }

        [TestElement("input[name=\"password\"]")]
        public IWebElement Password { get; set; }

        [TestElement(".btn-primary")]
        public IWebElement LogInBtn { get; set; }

        [TestElement(".btn-secundary")]
        public IWebElement LogOffBtn { get; set; }

        [TestElement(".alert")]
        public IWebElement ErrorMessage { get; set; }

        [TestElement("//*[@id=\"root\"]/div/div/h1", Type = SelectorType.XPath)]
        private IWebElement HelloWorld { get; set; }

        public void LogIn()
        {
            Username.SendKeys("admin");
            Password.SendKeys("admin");
          
            this.Click(x => x.LogInBtn);

            this.Sync(x => x.HelloWorld).WaitUntil(x => x.HelloWorld != null);

            HelloWorld.Should().NotBeNull();

            this.Sync(x => x.Username).WaitUntil(x => x.Username == null);


            Username.Should().BeNull();
        }
    }
}
