using Cybtans.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Benchmark
{
    public class ModelA
    {
        public int IntValue { get; set; } = 1;

        public string StringValue { get; set; } = "Hellow World";

        public float FloatValue { get; set; } = 55555.99999f;

        public double DoubleValue { get; set; } = Math.PI;

        public byte ByteValue { get; set; } = 0xFF;

        public double SmallDouble { get; set; } = 5;

        public double SmallDecimal { get; set; } = 700.50;

        public float SmallFloat { get; set; } = 50;

        public double SmallDouble2 { get; set; } = 5.5;

        public byte[] BufferValue { get; set; } = new byte[] { 0x01, 0x02, 0x03, 0x05, 0xFF };

        public int[] ArrayIntValue { get; set; } = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        public string[] ArrayStringValue { get; set; } = new string[] { "Baar", "Foo", "Delta" };

        public List<string> ListStringValue { get; set; } = new List<string>();

        public ModelB ModelBValue { get; set; }

        public List<ModelB> ModelBListValue { get; set; }

        public Dictionary<string, ModelB> MapValue { get; set; }
      
    }


    public class ModelB
    {        

        public int Id { get; set; }

        public string Name { get; set; }

        public bool Checked { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }     
      
    }    
}
