using Cybtans.Serialization;
using Cybtans.Serialization.Tests.Models;
using Cybtans.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Serialization.Tests.Serialization
{
    public class BinarySerializerTest
    {
        BinarySerializer _binarySerializer;
        private ITestOutputHelper _testOutput;
        private ModelA modelA;

        public BinarySerializerTest(ITestOutputHelper testOutput)
        {
            _binarySerializer = new BinarySerializer();
            _testOutput = testOutput;

            var modelB = new ModelB
            {
                Id = 1,
                Name = "Foo Bar ModelB",
                CreateDate = DateTime.Now
            };

            modelA = new ModelA
            {
                ModelBValue = modelB,
                GuidValue = Guid.NewGuid(),
                MapValue = new Dictionary<string, ModelB>
                {
                    ["1"] = modelB
                },
                ModelBListValue = Enumerable.Range(1, 100).Select(x => new ModelB
                {
                    Id = x,
                    Name = $"Foo Bar {x}",
                    CreateDate = DateTime.Now
                }).ToList(),
                ArrayIntValue = Enumerable.Range(1, 1000).ToArray(),
                ListStringValue = Enumerable.Range(1, 100).Select(x => x.ToString()).ToList()
            };
        }

        private List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            var brands = Enumerable.Range(1, 5).Select(i => new Brand { Id = i, Name = $"Brand {i}" }).ToList();
            var catalogs = Enumerable.Range(1, 3).Select(i => new Models.Catalog { Id = i, Name = $"Catalog {i}" });

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
        public void SeralizeTyped()
        {
            var sw = new Stopwatch();

            sw.Start();
            var buffer = _binarySerializer.Serialize(modelA);
            sw.Stop();

            _testOutput.WriteLine($"BINARY Serialize {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.NotNull(buffer);
            Assert.NotEmpty(buffer);

            sw.Start();
            var result = _binarySerializer.Deserialize<ModelA>(buffer);
            sw.Stop();

            _testOutput.WriteLine($"BINARY Deserialize {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.True(modelA.Equals(result));

            _testOutput.WriteLine($"Bytes BINARY:{buffer.Length} bytes");

        }

        [Fact]
        public void SeralizeUntyped()
        {
            var sw = new Stopwatch();

            sw.Start();
            var buffer = _binarySerializer.Serialize(modelA);
            sw.Stop();

            _testOutput.WriteLine($"BINARY Serialize {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.NotNull(buffer);
            Assert.NotEmpty(buffer);

            sw.Start();
            var result = (Dictionary<object, object>) _binarySerializer.Deserialize(buffer);
            sw.Stop();

            _testOutput.WriteLine($"BINARY Deserialize {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.NotNull(result);
            Assert.Equal(Convert.ToInt32(result["IntValue"]), modelA.IntValue);
            Assert.Equal((List<object>)result["ListStringValue"], modelA.ListStringValue);
        }

        [Fact]
        public void SerializeProducts()
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
        public void SerializeValidationResult()
        {
            var result = new ValidationResult("An error occurred while updating the entries. See the inner exception for details.\r\n----Inner Exception-- -\r\nSQLite Error 19: 'UNIQUE constraint failed: Ordes.Id'.\r\n");

            var bytes = BinaryConvert.Serialize(result);
            Assert.NotEmpty(bytes);

            var obj = BinaryConvert.Deserialize<ValidationResult>(bytes);
            Assert.NotNull(obj);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SerializeCompare(bool useNewtonsoft)
        {
            var sw = new Stopwatch();

            sw.Start();
            var buffer = _binarySerializer.Serialize(modelA);
            sw.Stop();

            _testOutput.WriteLine($"BINARY Serialize {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.NotNull(buffer);
            Assert.NotEmpty(buffer);

            sw.Start();
            var result = _binarySerializer.Deserialize<ModelA>(buffer);
            sw.Stop();

            _testOutput.WriteLine($"BINARY Deserialize {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.True(modelA.Equals(result));

            sw.Start();
            var json = useNewtonsoft ?
                JsonConvert.SerializeObject(modelA) :
                System.Text.Json.JsonSerializer.Serialize(modelA);
            var bytesJson = Encoding.UTF8.GetBytes(json);
            sw.Stop();

            var formatter = useNewtonsoft ? "Json.NET" : "System.Text.Json";
            _testOutput.WriteLine($"{formatter} Serialize {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            sw.Start();
            json = Encoding.UTF8.GetString(bytesJson);
            result = useNewtonsoft ?
                JsonConvert.DeserializeObject<ModelA>(json) :
                System.Text.Json.JsonSerializer.Deserialize<ModelA>(json);
            sw.Stop();

            _testOutput.WriteLine($"{formatter} Deserialize {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.True(modelA.Equals(result));

            _testOutput.WriteLine($"{formatter}:{bytesJson.Length} bytes BINARY:{buffer.Length} bytes");

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
