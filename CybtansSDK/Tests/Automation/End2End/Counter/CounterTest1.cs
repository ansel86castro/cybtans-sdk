using End2End.Components;
using FluentAssertions;
using Cybtans.Automation;
using Xunit;
using Cybtans.Automation.Xunit;

namespace End2End.Counter
{    

    [AutoSync]
    [Trait("Category", "CollectionFixture")]
    public class CounterTest1 : AutomationTest<CounterPage, CounterTest1.CounterFixture>
    {
        [DriverContext("Counter")]
        [PageUrl("/counter")]
        public class CounterFixture : AutomationFixture<CounterPage> { }

        public CounterTest1(CounterFixture fixture):base(fixture)
        {
            
        }

        [Fact]
        public void IncrementTo10()
        {
            for (int i = 0; i < 10; i++)
            {
                Page.IncButton.Click();
            }

            Page.Sync(x => x.CounterValue).Now();
            Page.CounterValue.Text.Should().Be("10");
        }
    }
}
