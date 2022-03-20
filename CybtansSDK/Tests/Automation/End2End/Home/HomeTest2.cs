using End2End.Components;
using FluentAssertions;
using Cybtans.Automation;
using Xunit;
using Cybtans.Automation.Xunit;
using Cybtans.Tests.Core.Xunit;

namespace End2End.Home
{
    [AutoSync]
    [Trait("Category", "Home")]
    public class HomeTest2 : AutomationTest<AppHomePage, HomeTest2.HomeFixture>
    {      

        [UseDriver("Home")]          
        public class HomeFixture : AutomationFixture<AppHomePage> { }

        public HomeTest2(HomeFixture fixture):base(fixture)          
        {     
        }

        [Fact(DisplayName = "Ensure Content is displayed correctly")]
        [TestOrder(2)]
        public void ShouldLoadContent()
        {
            Page.Content.Should().NotBeNull();

            Page.Content.HellowMessage.Text.Should().Be("Hello, world!");

            Page.Content.Links.Items.Should().HaveCount(3);
            Page.Content.Setup.Items.Should().HaveCount(3);
        }
    }
}
