using Cybtans.Tests.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Cybtans.Expressions;
using Newtonsoft.Json;

namespace Cybtans.Tests.Expressions
{
    public class LinqExpressionParserTest
    {
        IQueryable<Customer> _customers;

        public LinqExpressionParserTest()
        {
            _customers = Enumerable.Range(1, 10)
                .Select(i => new Customer
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    Name = $"Customer {i}",
                    CustomerProfile = new CustomerProfile
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Profile Customer {i}",
                        CreateDate = DateTime.Now
                    }
                }).ToList().AsQueryable();
        }

        [Fact]
        public void FilterByDateTimeJson()
        {
            var date = DateTime.Now;
            var filter = $"CreateDate le {JsonConvert.SerializeObject(date).Replace('"', '\'')}";
            var result = _customers.Where(filter);
            Assert.NotNull(result);
        }
    }
}
