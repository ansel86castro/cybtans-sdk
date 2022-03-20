using System;

namespace Cybtans.Automation.Xunit
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PageUrlAttribute : Attribute
    {
        public PageUrlAttribute(string url)
        {
            this.Url = url;
            IsRelative = string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Relative);
        }

        public string Url { get; }
        public bool IsRelative { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoSyncAttribute:Attribute
    {
        public bool WaitForElements { get; set; } = true;
    }
}
