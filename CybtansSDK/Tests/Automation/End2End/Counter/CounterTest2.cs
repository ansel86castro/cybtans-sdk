using End2End.Components;
using FluentAssertions;
using Cybtans.Automation;
using Xunit;
using Cybtans.Automation.Xunit;

namespace End2End.Counter
{     
    [Trait("Category", "CollectionFixture")]
    public class CounterTest2 : AutomationTest<CounterPage, CounterTest2.CounterFixture>
    {        
        [UseDriver("Counter")]       
        public class CounterFixture : AutomationFixture<CounterPage> { }

        public CounterTest2(CounterFixture fixture):base(fixture)
        {
            
        }

        [Fact]
        public void IncrementTo20()
        {
            for (int i = 0; i < 10; i++)
            {
                Page.IncButton.Click();
            }

            Page.Sync(x => x.CounterValue).Now();

            Page.CounterValue.Text.Should().Be("20");
        }
    }
}
