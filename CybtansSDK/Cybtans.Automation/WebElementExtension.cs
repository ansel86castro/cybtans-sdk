using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Cybtans.Automation
{
    public static class WebDriverExtension
    {
        public static void ScrollIntoView(this IWebDriver driver, IWebElement element, bool alignToTop = false)
        {
            const string script = "arguments[0].scrollIntoView(arguments[1])";
            var executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript(script, element, alignToTop);
        }

        public static void ScrollIntoViewIfNeeded(this IWebDriver driver, IWebElement element)
        {
            const string script = "arguments[0].scrollIntoViewIfNeeded()";
            var executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript(script, element);
        }

        public static bool IsChecked(this IWebElement element)
        {
            return element.GetAttribute("checked") == "true";
        }

        public static string Value(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        public static void Value(this IWebElement element, IWebDriver driver, string value)
        {
            const string script = "arguments[0].value = arguments[1]";            
            var executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript(script, element, value);            
        }


        public static void SendKeys(this IWebElement element, string value, int delayMs)
        {
            element.Clear();

            for (int i = 0; i < value.Length; i++)
            {
                if(delayMs > 0)
                {
                    Thread.Sleep(delayMs);
                }                        
                element.SendKeys(value[i].ToString());
            }
        }

    }
}
