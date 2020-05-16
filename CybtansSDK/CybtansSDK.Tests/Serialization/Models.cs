using CybtansSdk.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace CybtansSDK.Tests.Serialization
{
    public class ModelA: IEquatable<ModelA>
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

        public bool Equals([AllowNull] ModelA other)
        {
            if (other == null)
                return false;

            return IntValue == other.IntValue &&
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
            if(obj is ModelA)
                return Equals((ModelA)obj);
            return base.Equals(obj);
        }
    }    


    public class ModelB:IEquatable<ModelB> , IPropertiesAccesorProvider
    {
        private static readonly ModelBAccesor __accesor = new ModelBAccesor();

        public int  Id { get; set; }

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

        public IPropertiesAccesor GetAccesor()
        {
            return __accesor;
        }
    }

    public sealed class ModelBAccesor : IPropertiesAccesor
    {
        public const int Id = 1;
        public const int Name = 2;
        public const int Checked =3;
        public const int CreateDated = 4;
        public const int UpdateDate = 5;

        public readonly int[] _props = new[]
        {
            Id, Name, Checked, CreateDated, UpdateDate
        };

        public int[] GetPropertyCodes() => _props;

        public Type GetPropertyType(int propertyCode)
        {
            return propertyCode switch
            {
                Id => typeof(int),
                Name => typeof(string),
                Checked => typeof(bool),
                CreateDated => typeof(DateTime),
                UpdateDate => typeof(DateTime?),
                _ => throw new InvalidOperationException("property code not supported"),
            };
        }

        public object GetValue(object target, int propertyCode)
        {
            ModelB obj = (ModelB)target;
            return propertyCode switch
            {
                Id => obj.Id,
                Name => obj.Name,
                Checked => obj.Checked,
                CreateDated => obj.CreateDate,
                UpdateDate => obj.UpdateDate,
                _ => throw new InvalidOperationException("property code not supported"),
            };
        }

        public void SetValue(object target, int propertyCode, object value)
        {
            ModelB obj = (ModelB)target;
            switch (propertyCode)
            {
                case Id:  obj.Id = (int)value;break;
                case Name:  obj.Name = (string)value; break;
                case Checked:  obj.Checked = (bool)value; break;
                case CreateDated:  obj.CreateDate = (DateTime)value; break;
                case UpdateDate:  obj.UpdateDate = (DateTime?)value; break;
                default: throw new InvalidOperationException("property code not supported");
            }
        }
    }
}
