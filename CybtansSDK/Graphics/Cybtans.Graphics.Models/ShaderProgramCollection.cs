using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class ShaderProgramCollection : IReflectorMetadataProvider
	{
		private static readonly ShaderProgramCollectionAccesor __accesor = new ShaderProgramCollectionAccesor();
		
		public List<ShaderProgramDto> Programs {get; set;}
		
		public string DefaultProgram {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class ShaderProgramCollectionAccesor : IReflectorMetadata
	{
		public const int Programs = 1;
		public const int DefaultProgram = 2;
		private readonly int[] _props = new []
		{
			Programs,DefaultProgram
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Programs => "Programs",
		       DefaultProgram => "DefaultProgram",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Programs" => Programs,
		        "DefaultProgram" => DefaultProgram,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Programs => typeof(List<ShaderProgramDto>),
		        DefaultProgram => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    ShaderProgramCollection obj = (ShaderProgramCollection)target;
		    return propertyCode switch
		    {
		        Programs => obj.Programs,
		        DefaultProgram => obj.DefaultProgram,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    ShaderProgramCollection obj = (ShaderProgramCollection)target;
		    switch (propertyCode)
		    {
		        case Programs:  obj.Programs = (List<ShaderProgramDto>)value;break;
		        case DefaultProgram:  obj.DefaultProgram = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
