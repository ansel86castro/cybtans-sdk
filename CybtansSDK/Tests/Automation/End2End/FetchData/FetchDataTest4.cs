//using End2End.Components;
//using FluentAssertions;
//using Cybtans.Automation;
//using Xunit;

//namespace End2End.FetchData
//{
//    [Trait("Category", "FetchData")]
//    [UseDriver("Login", DisableNavigation = true)]
//    [WaitForBarrier(nameof(FetchDataTest3))]
//    public class FetchDataTest4 : PageTest<FetchDataPage>
//    {
//        public FetchDataTest4(PageFixture<FetchDataPage> fixture) : base(fixture) { }

//        [Fact(DisplayName = "Ensure next page is loaded")]
//        public void ShouldLoadNextPage()
//        {
//            //Ensure the element is syncronized and is clickable by scrolling into view
//            Page.Sync(x => x.Pagination).WaitUntilExist();

//            Page.Pagination.Click(x => x.NextPage);

//            Page.SyncAll().WaitUntil(x => !x.IsLoading);

//            //example running an script 
//            var result = (long)Page.ExecuteScript(@"
//                let div = document.getElementsByClassName('container')[1];
//                div.append('Hellow World');
//                return 5;
//            ");

//            Assert.Equal(5, result);

//            Page.Table.RowsCount.Should().Be(5);
//        }

//    }
//}
