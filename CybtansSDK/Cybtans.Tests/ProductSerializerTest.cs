using Cybtans.Serialization.Tests.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Serialization.Tests
{
    public  class ProductSerializerTest
    {
        BinarySerializer _binarySerializer;
        private ITestOutputHelper _testOutput;

        public ProductSerializerTest(ITestOutputHelper testOutput)
        {
            _binarySerializer = new BinarySerializer();
            _testOutput = testOutput;
        }

        private List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            var brands = Enumerable.Range(1, 5).Select(i => new Brand { Id = i, Name = $"Brand {i}" }).ToList();
            var catalogs = Enumerable.Range(1, 3).Select(i => new Models.Catalog {Id = i, Name = $"Catalog {i}" });

            var rand = new Random();
            foreach (var catalog in catalogs)
            {
                products.AddRange(Enumerable.Range(1, 10)
                     .Select(i => new Product
                     {
                         Brand = brands[i % 5],
                         Catalog = catalog,
                         AvalaibleStock = rand.Next(1000),
                         CreateDate = DateTime.Now,
                         Name = $"Product {catalog.Id}-{i}",
                         Price = i % 2 == 0 ? i : float.Parse($"{rand.Next(100, 500)}.{rand.Next(0, 100)}"),
                         RestockThreshold = 5,
                         Description = $"Description for Product {catalog.Id}{i}",
                         BrandId = brands[i % 5].Id,
                         CatalogId = catalog.Id
                     }));

            }

            return products;
        }

        [Fact]
        public void SerializeBinary()
        {
            var products = GetProducts();

            var sw = new Stopwatch();
            sw.Start();

            var buffer = _binarySerializer.Serialize(products);

            sw.Stop();
            _testOutput.WriteLine($"Binary serialize time {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");
            _testOutput.WriteLine($"Binary data size {buffer.Length} bytes");

            Assert.NotNull(buffer);
            Assert.NotEmpty(buffer);

            sw.Restart();
            var result = _binarySerializer.Deserialize<List<Product>>(buffer);
            sw.Stop();
            _testOutput.WriteLine($"Binary desserialize time {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.NotNull(result);
            Assert.Equal(products.Count, result.Count);

            AssertProducts(products, result);
        }

        [Fact]
        public void SerializeJson()
        {
            var products = GetProducts();

            var sw = new Stopwatch();
            sw.Start();

            var buffer = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(products);            

            sw.Stop();
            _testOutput.WriteLine($"Json serialize time {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");
            _testOutput.WriteLine($"Json data size {buffer.Length} bytes");

            Assert.NotNull(buffer);
            Assert.NotEmpty(buffer);

            sw.Restart();
            var result = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(buffer);
            sw.Stop();
            _testOutput.WriteLine($"Json desserialize time {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.NotNull(result);
            Assert.Equal(products.Count, result.Count);

            AssertProducts(products, result);
        }

        [Fact]
        public void SerializeNewtonsoftJson()
        {
            var products = GetProducts();

            var sw = new Stopwatch();
            sw.Start();

            var json = JsonConvert.SerializeObject(products);
            var buffer = Encoding.UTF8.GetBytes(json);

            sw.Stop();
            _testOutput.WriteLine($"Newtonsoft serialize time {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");
            _testOutput.WriteLine($"Newtonsoft data size {buffer.Length} bytes");

            Assert.NotNull(buffer);
            Assert.NotEmpty(buffer);

            sw.Restart();
            var result = JsonConvert.DeserializeObject<List<Product>>(json);
            sw.Stop();
            _testOutput.WriteLine($"Newtonsoft desserialize time {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.NotNull(result);
            Assert.Equal(products.Count, result.Count);

            AssertProducts(products, result);
        }

        private void AssertProducts(List<Product> products, List<Product> result)
        {
            var productAcccesor = new ProductAccesor();
            for (int i = 0; i < result.Count; i++)
            {
                var r = result[i];
                var p = products[i];

                var properties = productAcccesor.GetPropertyCodes();
                foreach (var code in properties)
                {
                    if (code == ProductAccesor.Brand)
                    {
                        var a = (Brand)productAcccesor.GetValue(r, code);
                        var b = (Brand)productAcccesor.GetValue(p, code);
                        AssertBrands(a, b);
                    }
                    else if (code == ProductAccesor.Catalog)
                    {
                        var a = (Catalog)productAcccesor.GetValue(r, code);
                        var b = (Catalog)productAcccesor.GetValue(p, code);
                        AssertCatalogs(a, b);
                    }
                    else
                    {
                        Assert.Equal(productAcccesor.GetValue(p, code), productAcccesor.GetValue(r, code));
                    }
                }
            }
        }

        private void AssertCatalogs(Catalog a, Catalog b)
        {
            Assert.Equal(b.Id, a.Id);
            Assert.Equal(b.Name, a.Name);
        }

        private void AssertBrands(Brand a, Brand b)
        {
            Assert.Equal(b.Id, a.Id);
            Assert.Equal(b.Name, a.Name);
        }
    }
}
