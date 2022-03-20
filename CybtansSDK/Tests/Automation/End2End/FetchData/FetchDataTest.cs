//using End2End.Components;
//using FluentAssertions;
//using Cybtans.Automation;
//using Xunit;

//namespace End2End.FetchData
//{
//    [Trait("Category", "FetchData")]
//    [UseDriver("Login")]
//    [Barrier]
//    public class FetchDataTest : PageTest<FetchDataPage>
//    {
//        public FetchDataTest(PageFixture<FetchDataPage> fixture) : base(fixture) { }

//        [Fact(DisplayName = "Ensure the rows are loaded")]
//        public void ShouldLoadRows()
//        {
//            Page.Table.Should().NotBeNull();
//            Page.Pagination.Should().NotBeNull();

//            Page.Table.Sync(x => x.Rows).WaitUntil(table => !table.IsEmpty);

//            Page.Table.RowsCount.Should().Be(5);
//        }

//    }
//}
