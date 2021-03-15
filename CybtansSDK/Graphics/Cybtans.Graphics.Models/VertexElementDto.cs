using System;
using Cybtans.Serialization;

namespace Cybtans.Graphics.Models
{
	public partial class VertexElementDto : IReflectorMetadataProvider
	{
		private static readonly VertexElementDtoAccesor __accesor = new VertexElementDtoAccesor();
		
		public short Offset {get; set;}
		
		public short? Stream {get; set;}
		
		public string Semantic {get; set;}
		
		public byte? UsageIndex {get; set;}
		
		public VertexElementFormat Format {get; set;}
		
		public int Size {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class VertexElementDtoAccesor : IReflectorMetadata
	{
		public const int Offset = 1;
		public const int Stream = 2;
		public const int Semantic = 3;
		public const int UsageIndex = 4;
		public const int Format = 5;
		public const int Size = 6;
		private readonly int[] _props = new []
		{
			Offset,Stream,Semantic,UsageIndex,Format,Size
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Offset => "Offset",
		       Stream => "Stream",
		       Semantic => "Semantic",
		       UsageIndex => "UsageIndex",
		       Format => "Format",
		       Size => "Size",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Offset" => Offset,
		        "Stream" => Stream,
		        "Semantic" => Semantic,
		        "UsageIndex" => UsageIndex,
		        "Format" => Format,
		        "Size" => Size,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Offset => typeof(short),
		        Stream => typeof(short?),
		        Semantic => typeof(string),
		        UsageIndex => typeof(byte?),
		        Format => typeof(VertexElementFormat),
		        Size => typeof(int),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    VertexElementDto obj = (VertexElementDto)target;
		    return propertyCode switch
		    {
		        Offset => obj.Offset,
		        Stream => obj.Stream,
		        Semantic => obj.Semantic,
		        UsageIndex => obj.UsageIndex,
		        Format => obj.Format,
		        Size => obj.Size,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    VertexElementDto obj = (VertexElementDto)target;
		    switch (propertyCode)
		    {
		        case Offset:  obj.Offset = (short)value;break;
		        case Stream:  obj.Stream = (short?)value;break;
		        case Semantic:  obj.Semantic = (string)value;break;
		        case UsageIndex:  obj.UsageIndex = (byte?)value;break;
		        case Format:  obj.Format = (VertexElementFormat)value;break;
		        case Size:  obj.Size = (int)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
