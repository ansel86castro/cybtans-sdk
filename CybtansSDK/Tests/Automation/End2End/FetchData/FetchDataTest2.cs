//using End2End.Components;
//using FluentAssertions;
//using Cybtans.Automation;
//using Xunit;

//namespace End2End.FetchData
//{
//    [Trait("Category", "FetchData")]
//    [UseDriver("Login", DisableNavigation = true)]
//    [WaitForBarrier(nameof(FetchDataTest))]
//    public class FetchDataTest2 : PageTest<FetchDataPage>
//    {
//        public FetchDataTest2(PageFixture<FetchDataPage> fixture) : base(fixture) { }

//        [Fact(DisplayName = "Ensure the title and description is loaded")]
//        public void ShouldShowTitle()
//        {
//            Page.Title.Text.Should().Be("Weather forecast");
//            Page.Description.Text.Should().Be("This component demonstrates fetching data from the server and working with URL parameters.");
//        }
//    }
//}
