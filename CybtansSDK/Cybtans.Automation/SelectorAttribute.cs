using OpenQA.Selenium;
using System;

namespace Cybtans.Automation
{
    public enum SelectorType { None, Id, ClassName, XPath, Css, Tag }

    public class TestElementAttribute : Attribute
    {
        public TestElementAttribute(string selector)
        {
            Type = SelectorType.Css;
            Selector = selector;
        }

        public TestElementAttribute()
        {
            Type = SelectorType.None;
        }

        public bool IsRequired { get; set; }

        public string Selector { get; }

        public SelectorType Type { get; set; }

        public By GetSelector()
        {
            switch (Type)
            {
                case SelectorType.Id: return By.Id(Selector);
                case SelectorType.ClassName: return By.ClassName(Selector);
                case SelectorType.Css: return By.CssSelector(Selector);
                case SelectorType.Tag: return By.TagName(Selector);
                case SelectorType.XPath: return By.XPath(Selector);
                default: return null;
            }
        }
    }

}
