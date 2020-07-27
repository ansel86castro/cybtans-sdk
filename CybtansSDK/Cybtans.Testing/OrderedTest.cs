using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Testing
{
    [TestCaseOrderer("Cybtans.Testing.DependencyTestCaseOrderer", "Cybtans.Testing")]
    public class OrderedTest : TestBase
    {
        public OrderedTest() : base(null) { }

        public OrderedTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }
    }

    [TestCaseOrderer("Cybtans.Testing.DependencyTestCaseOrderer", "Cybtans.Testing")]
    public class OrderedTest<TFixture> : TestBase<TFixture>
        where TFixture : BaseFixture
    {
        public OrderedTest(TFixture fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper)
        {

        }

        public OrderedTest(TFixture fixture) : base(fixture, null)
        {

        }
    }
}
