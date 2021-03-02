using Cybtans.Tests.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Cybtans.Expressions;
using Newtonsoft.Json;
using System.Linq.Expressions;

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
                    Name = i < 9 ? $"Customer {i}" : $"Customer '{i}",
                    CustomerProfile = new CustomerProfile
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Profile Customer {i}",
                        CreateDate = DateTime.Now
                    },
                    Orders = i > 5 ? new List<Order>() : new List<Order>
                    {
                        new Order
                        {
                             Description =  "Test",
                             Id =Guid.NewGuid(),
                             Items = new List<OrderItem>
                             {
                                 new OrderItem
                                 {
                                      Id = Guid.NewGuid(),
                                      ProductName = "TestProduct "+i,
                                      Price = i
                                 }
                             }
                        },
                        new Order
                        {
                            Description = i.ToString(),
                             Id =Guid.NewGuid(),
                        }
                    }
                }).ToList().AsQueryable();
        }

        [Fact]
        public void SimpleFilter()
        {
            var result = _customers.Where("name = 'Customer 1'").ToList();           
            Assert.Single(result);

            result = _customers.Where("name like '%1%'").ToList();            
            Assert.Equal(2, result.Count);

            result = _customers.Where("name = 'Customer 1' or name = 'Customer 2'").ToList();
            Assert.Equal(2, result.Count);
        }


        [Fact]
        public void ChaningFilter()
        {
            var result = _customers.Where("customerProfile.name = 'Profile Customer 1'").ToList();            
            Assert.Single(result);

        }

        [Fact]
        public void FilterByDateTimeJson()
        {
            var date = DateTime.Now;
            var filter = $"CreateDate > {JsonConvert.SerializeObject(date).Replace('"', '\'')}";
            var result = _customers.Where(filter);
            Assert.NotNull(result);
        }

        [Fact]
        public void AllOrders()
        {         
           var result = _customers.Where("orders.size() > 0 and orders.all(description = '1' or description = '2')").ToList();
            Assert.Empty(result);

            result = _customers.Where("orders.all(description = '1')").ToList();
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void AnyOrders()
        {           
            var result = _customers.Where("orders.any(description = '1' or description = '2')").ToList();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void CountOrders()
        {
            var result = _customers.Where("orders.count(description = '1' or description = '2') = 1").ToList();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void SizeOrders()
        {
            var result = _customers.Where("orders.size() > 0").ToList();
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void CountOrdersAndFilterCustomer()
        {
            var result = _customers.Where("name like '%1%' and orders.count(description = '1' or description = '2') = 1").ToList();
            Assert.Single(result);
        }

        [Fact]
        public void StringScapeFilter()
        {
            var result = _customers.Where("name ='Customer \\'10'").ToList();
            Assert.Single(result);

            result = _customers.Where("name like '%Customer \\'1%'").ToList();
            Assert.Single(result);
            
        }
    }
}
