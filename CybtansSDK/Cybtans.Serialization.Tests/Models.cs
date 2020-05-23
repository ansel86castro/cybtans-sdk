using Cybtans.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Cybtans.Serialization.Tests.Serialization
{
    public class ModelA : IEquatable<ModelA>
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

        public Guid GuidValue { get; set; } 

        public byte[] BufferValue { get; set; } = new byte[] { 0x01, 0x02, 0x03, 0x05, 0xFF };

        public int[] ArrayIntValue { get; set; } = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        public string[] ArrayStringValue { get; set; } = new string[] { "Baar", "Foo", "Delta" };

        public List<string> ListStringValue { get; set; } = new List<string>();

        public ModelB ModelBValue { get; set; }

        public List<ModelB> ModelBListValue { get; set; }

        public Dictionary<string, ModelB> MapValue { get; set; }

        public bool Equals([AllowNull] ModelA other)
        {
            if (other == null)
                return false;

            return IntValue == other.IntValue &&
                GuidValue == other.GuidValue &&
                StringValue == other.StringValue &&
                DoubleValue == other.DoubleValue &&
                FloatValue == other.FloatValue &&
                ByteValue == other.ByteValue &&
                SmallDouble == other.SmallDouble &&
                SmallDecimal == other.SmallDecimal &&
                SmallFloat == other.SmallFloat &&
                SmallDouble2 == other.SmallDouble2 &&
                BufferValue.SequenceEqual(other.BufferValue) &&
                ArrayIntValue.SequenceEqual(other.ArrayIntValue) &&
                ArrayStringValue.SequenceEqual(other.ArrayStringValue) &&
                ModelBValue.Equals(other.ModelBValue) &&
                ModelBListValue.SequenceEqual(other.ModelBListValue) &&
                MapValue.SequenceEqual(other.MapValue) &&
                ListStringValue.SequenceEqual(other.ListStringValue);

        }

        public override bool Equals(object obj)
        {
            if (obj is ModelA)
                return Equals((ModelA)obj);
            return base.Equals(obj);
        }
    }


    public class ModelB : IEquatable<ModelB>
    {        

        public int Id { get; set; }

        public string Name { get; set; }

        public bool Checked { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool Equals([AllowNull] ModelB other)
        {
            if (other == null)
                return false;

            return Id == other.Id
                && Name == other.Name
                && Checked == other.Checked
                && CreateDate == other.CreateDate
                && UpdateDate == other.UpdateDate;
        }

        public override bool Equals(object obj)
        {
            return obj is ModelB ? Equals((ModelB)obj) : base.Equals(obj);
        }
      
    }   
}
