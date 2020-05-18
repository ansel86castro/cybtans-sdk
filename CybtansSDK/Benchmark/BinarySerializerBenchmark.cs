using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Cybtans.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benchmark
{  
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]   
    [RPlotExporter]
    public class BinarySerializerBenchmark
    {       
        private ModelA _modelA;
        byte[] _buffer;
        string _json;

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

            _json = System.Text.Json.JsonSerializer.Serialize(_modelA);
            BinarySerializer binarySerializer = new BinarySerializer();
            _buffer = binarySerializer.Serialize(_modelA);
        }

        [Benchmark]
        public byte[] Serialize_Cybtans_Binary()
        {
            BinarySerializer binarySerializer = new BinarySerializer();
            return binarySerializer.Serialize(_modelA);
        }

        [Benchmark]
        public ModelA Deserialize_Cybtan_Binary()
        {
            BinarySerializer binarySerializer = new BinarySerializer();
            return binarySerializer.Deserialize<ModelA>(_buffer);
        }

        [Benchmark]
        public byte[] Serialize_NewtonSoft()
        {
            var json = JsonConvert.SerializeObject(_modelA);
            return Encoding.UTF8.GetBytes(json);
        }

        [Benchmark]
        public ModelA Deserialize_NewtonSoft()
        {           
            return JsonConvert.DeserializeObject<ModelA>(_json);            
        }

        [Benchmark()]
        public byte[] Serialize_System_Text_Json()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_modelA);
            return Encoding.UTF8.GetBytes(json);
        }

        [Benchmark]
        public ModelA Deserialize_System_Text_Json()
        {            
            return System.Text.Json.JsonSerializer.Deserialize<ModelA>(_json);
        }


    }

    
}
