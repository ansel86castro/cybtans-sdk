using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using Cybtans.Serialization;
using Cybtans.Serialization.Tests.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Benchmark
{        
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Throughput, RuntimeMoniker.NetCoreApp31)]       
    public class BinarySerializerBenchmark
    {       
        private ModelA _modelA;
        private List<Product> _products;
        
        [GlobalSetup]
        public void Setup()
        {
            var modelB = new ModelB
            {
                Id = 1,
                Name = "Foo Bar ModelB",
                CreateDate = DateTime.Now
            };

            _modelA = new ModelA
            {
                ModelBValue = modelB,
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

            _products = Product.GetProducts();
        }
       
        [Benchmark]
        public void BinarySerialize()
        {
            var buffer = BinaryConvert.Serialize(_modelA);
            var result = BinaryConvert.Deserialize<ModelA>(buffer);

            Assert(result);
        }

        [Benchmark]
        public void BinarySerializeWithMetadataProvider()
        {
            var buffer = BinaryConvert.Serialize(_products);
            var result = BinaryConvert.Deserialize<List<Product>>(buffer);

            AssertProducts(result);
        }


        [Benchmark]
        public void JsonSerialize()
        {
            var buffer = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(_modelA);
            var result = System.Text.Json.JsonSerializer.Deserialize<ModelA>(buffer);

            Assert(result);
        }

        [Benchmark]
        public void JsonSerializeWithMetadataProvider()
        {
            var buffer = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(_products);
            var result = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(buffer);

            AssertProducts(result);
        }

        [Benchmark]
        public void NewtonSoftSerialize()
        {
            var json = JsonConvert.SerializeObject(_modelA);            
            var result = JsonConvert.DeserializeObject<ModelA>(json);

            Assert(result);
        }

        private void Assert(ModelA result)
        {
            Contract.Assert(result != _modelA);
            Contract.Assert(result.IntValue == _modelA.IntValue);
            Contract.Assert(result.StringValue == _modelA.StringValue);
        }


        private void AssertProducts(List<Product> result)
        {
            Contract.Assert(result != null && result.Count == _products.Count);

            var productAcccesor = new ProductAccesor();
            for (int i = 0; i < result.Count; i++)
            {
                var r = result[i];
                var p = _products[i];

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
                        var a = productAcccesor.GetValue(p, code);
                        var b = productAcccesor.GetValue(r, code);
                        Contract.Assert((a == null && b == null) || (a?.Equals(b) ?? false));
                    }
                }
            }
        }

        private void AssertCatalogs(Catalog a, Catalog b)
        {
            Contract.Assert(b.Id == a.Id);
            Contract.Assert(b.Name == a.Name);
        }

        private void AssertBrands(Brand a, Brand b)
        {
            Contract.Assert(b.Id == a.Id);
            Contract.Assert(b.Name == a.Name);
        }

    }

    
}
