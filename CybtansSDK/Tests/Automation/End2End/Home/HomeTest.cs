using End2End.Components;
using FluentAssertions;
using Cybtans.Automation;
using Xunit;
using Cybtans.Automation.Xunit;
using Cybtans.Tests.Core.Xunit;

namespace End2End.Home
{


    [Trait("Category", "Home")]
    public class HomeTest : IClassFixture<HomeTest.HomeFixture>
    {
        [DriverContext("Home")]
        [PageUrl("")]
        public class HomeFixture : AutomationFixture<AppHomePage> { }

        public AppHomePage Page { get; }

        public HomeTest(HomeFixture fixture)
        {
            Page = fixture.Page;
        }

        [Fact(DisplayName = "Ensure Header is displayed correctly")]
        [TestOrder(1)]
        public void ShouldLoadHeader()
        {
            Page.Header.Should().NotBeNull();

            Page.Header.BrandText.Should().Be("App");
            Page.Header.Links.Should().HaveCount(4);

            Page.Header.Links[0].Text.Should().Be("Home");
            Page.Header.Links[1].Text.Should().Be("Counter");
            Page.Header.Links[2].Text.Should().Be("Fetch data");
            Page.Header.Links[3].Text.Should().Be("Login");
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
