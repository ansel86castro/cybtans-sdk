//using End2End.Components;
//using FluentAssertions;
//using Cybtans.Automation;
//using Attica.Tests.Core.Xunit;
//using Xunit;

//namespace End2End.FetchData
//{
//    [Trait("Category", "FetchData")]
//    [UseDriver("Login", DisableNavigation = true)]
//    [WaitForBarrier(nameof(FetchDataTest))]
//    [Barrier]
//    public class FetchDataTest3 : PageTest<FetchDataPage>
//    {
//        public FetchDataTest3(PageFixture<FetchDataPage> fixture) : base(fixture) { }

//        [Fact(DisplayName = "Ensure next page is loaded")]
//        public void ShouldLoadNextPage()
//        {
//            //Ensure the element is syncronized and is clickable by scrolling into view
//            Page.LoadNext();

//            Page.Table.RowsCount.Should().Be(5);
//        }

//        [Fact(DisplayName = "Ensure previous page is loaded")]
//        [DependsOn(nameof(ShouldLoadNextPage))]
//        public void ShouldLoadPreviousPage()
//        {
//            //Ensure the element is syncronized and is clickable by scrolling into view
//            Page.LoadPrevious();

//            Page.Table.RowsCount.Should().Be(5);
//        }
//    }
//}
