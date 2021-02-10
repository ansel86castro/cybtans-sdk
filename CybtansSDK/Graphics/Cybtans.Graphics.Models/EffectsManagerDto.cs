using System;
using Cybtans.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Graphics.Models
{
	public partial class EffectsManagerDto : IReflectorMetadataProvider
	{
		private static readonly EffectsManagerDtoAccesor __accesor = new EffectsManagerDtoAccesor();
		
		[Required]
		public Dictionary<string,ShaderProgramDto> Programs {get; set;}
		
		[Required]
		public Dictionary<string,EffectDto> Effects {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class EffectsManagerDtoAccesor : IReflectorMetadata
	{
		public const int Programs = 1;
		public const int Effects = 2;
		private readonly int[] _props = new []
		{
			Programs,Effects
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Programs => "Programs",
		       Effects => "Effects",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Programs" => Programs,
		        "Effects" => Effects,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Programs => typeof(Dictionary<string,ShaderProgramDto>),
		        Effects => typeof(Dictionary<string,EffectDto>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    EffectsManagerDto obj = (EffectsManagerDto)target;
		    return propertyCode switch
		    {
		        Programs => obj.Programs,
		        Effects => obj.Effects,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    EffectsManagerDto obj = (EffectsManagerDto)target;
		    switch (propertyCode)
		    {
		        case Programs:  obj.Programs = (Dictionary<string,ShaderProgramDto>)value;break;
		        case Effects:  obj.Effects = (Dictionary<string,EffectDto>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
