using System;
using Cybtans.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Graphics.Models
{
	public partial class ShaderProgramDto : IReflectorMetadataProvider
	{
		private static readonly ShaderProgramDtoAccesor __accesor = new ShaderProgramDtoAccesor();
		
		[Required]
		public string Name {get; set;}
		
		[Required]
		public ShaderDto VertexShader {get; set;}
		
		[Required]
		public ShaderDto FragmentShader {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class ShaderProgramDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int VertexShader = 2;
		public const int FragmentShader = 3;
		private readonly int[] _props = new []
		{
			Name,VertexShader,FragmentShader
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       VertexShader => "VertexShader",
		       FragmentShader => "FragmentShader",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Name" => Name,
		        "VertexShader" => VertexShader,
		        "FragmentShader" => FragmentShader,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Name => typeof(string),
		        VertexShader => typeof(ShaderDto),
		        FragmentShader => typeof(ShaderDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    ShaderProgramDto obj = (ShaderProgramDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        VertexShader => obj.VertexShader,
		        FragmentShader => obj.FragmentShader,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    ShaderProgramDto obj = (ShaderProgramDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case VertexShader:  obj.VertexShader = (ShaderDto)value;break;
		        case FragmentShader:  obj.FragmentShader = (ShaderDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
