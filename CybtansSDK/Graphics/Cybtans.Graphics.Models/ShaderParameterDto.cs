using System;
using Cybtans.Serialization;

namespace Cybtans.Graphics.Models
{
	public partial class ShaderParameterDto : IReflectorMetadataProvider
	{
		private static readonly ShaderParameterDtoAccesor __accesor = new ShaderParameterDtoAccesor();
		
		public string Target {get; set;}
		
		public string Property {get; set;}
		
		public string Type {get; set;}
		
		public string Path {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class ShaderParameterDtoAccesor : IReflectorMetadata
	{
		public const int Target = 1;
		public const int Property = 2;
		public const int Type = 3;
		public const int Path = 4;
		private readonly int[] _props = new []
		{
			Target,Property,Type,Path
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Target => "Target",
		       Property => "Property",
		       Type => "Type",
		       Path => "Path",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Target" => Target,
		        "Property" => Property,
		        "Type" => Type,
		        "Path" => Path,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Target => typeof(string),
		        Property => typeof(string),
		        Type => typeof(string),
		        Path => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    ShaderParameterDto obj = (ShaderParameterDto)target;
		    return propertyCode switch
		    {
		        Target => obj.Target,
		        Property => obj.Property,
		        Type => obj.Type,
		        Path => obj.Path,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    ShaderParameterDto obj = (ShaderParameterDto)target;
		    switch (propertyCode)
		    {
		        case Target:  obj.Target = (string)value;break;
		        case Property:  obj.Property = (string)value;break;
		        case Type:  obj.Type = (string)value;break;
		        case Path:  obj.Path = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
