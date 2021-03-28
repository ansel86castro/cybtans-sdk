using System;
using Cybtans.Serialization;

namespace Cybtans.Graphics.Models
{
	public partial class CubeMapDto : IReflectorMetadataProvider
	{
		private static readonly CubeMapDtoAccesor __accesor = new CubeMapDtoAccesor();
		
		public string PositiveX {get; set;}
		
		public string NegativeX {get; set;}
		
		public string PositiveY {get; set;}
		
		public string NegativeY {get; set;}
		
		public string PositiveZ {get; set;}
		
		public string NegativeZ {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class CubeMapDtoAccesor : IReflectorMetadata
	{
		public const int PositiveX = 1;
		public const int NegativeX = 2;
		public const int PositiveY = 3;
		public const int NegativeY = 4;
		public const int PositiveZ = 5;
		public const int NegativeZ = 6;
		private readonly int[] _props = new []
		{
			PositiveX,NegativeX,PositiveY,NegativeY,PositiveZ,NegativeZ
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       PositiveX => "PositiveX",
		       NegativeX => "NegativeX",
		       PositiveY => "PositiveY",
		       NegativeY => "NegativeY",
		       PositiveZ => "PositiveZ",
		       NegativeZ => "NegativeZ",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "PositiveX" => PositiveX,
		        "NegativeX" => NegativeX,
		        "PositiveY" => PositiveY,
		        "NegativeY" => NegativeY,
		        "PositiveZ" => PositiveZ,
		        "NegativeZ" => NegativeZ,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        PositiveX => typeof(string),
		        NegativeX => typeof(string),
		        PositiveY => typeof(string),
		        NegativeY => typeof(string),
		        PositiveZ => typeof(string),
		        NegativeZ => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    CubeMapDto obj = (CubeMapDto)target;
		    return propertyCode switch
		    {
		        PositiveX => obj.PositiveX,
		        NegativeX => obj.NegativeX,
		        PositiveY => obj.PositiveY,
		        NegativeY => obj.NegativeY,
		        PositiveZ => obj.PositiveZ,
		        NegativeZ => obj.NegativeZ,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    CubeMapDto obj = (CubeMapDto)target;
		    switch (propertyCode)
		    {
		        case PositiveX:  obj.PositiveX = (string)value;break;
		        case NegativeX:  obj.NegativeX = (string)value;break;
		        case PositiveY:  obj.PositiveY = (string)value;break;
		        case NegativeY:  obj.NegativeY = (string)value;break;
		        case PositiveZ:  obj.PositiveZ = (string)value;break;
		        case NegativeZ:  obj.NegativeZ = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
