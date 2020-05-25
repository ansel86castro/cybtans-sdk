using Cybtans.Serialization;
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SeralizeTyped(bool useNewtonsoft)
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
            dynamic result = (dynamic) _binarySerializer.Deserialize(buffer);
            sw.Stop();

            _testOutput.WriteLine($"BINARY Deserialize {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");

            Assert.NotNull(result);
            Assert.Equal(result.IntValue, modelA.IntValue);
            Assert.Equal(result.ListStringValue, modelA.ListStringValue);
        }
    
            
    }
}
