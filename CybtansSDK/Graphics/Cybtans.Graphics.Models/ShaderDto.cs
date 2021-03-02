using System;
using Cybtans.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Graphics.Models
{
	public partial class ShaderDto : IReflectorMetadataProvider
	{
		private static readonly ShaderDtoAccesor __accesor = new ShaderDtoAccesor();
		
		public Dictionary<string,string> Inputs {get; set;}
		
		public Dictionary<string,ShaderParameterDto> Parameters {get; set;}
		
		[Required]
		public string Source {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class ShaderDtoAccesor : IReflectorMetadata
	{
		public const int Inputs = 1;
		public const int Parameters = 3;
		public const int Source = 4;
		private readonly int[] _props = new []
		{
			Inputs,Parameters,Source
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Inputs => "Inputs",
		       Parameters => "Parameters",
		       Source => "Source",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Inputs" => Inputs,
		        "Parameters" => Parameters,
		        "Source" => Source,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Inputs => typeof(Dictionary<string,string>),
		        Parameters => typeof(Dictionary<string,ShaderParameterDto>),
		        Source => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    ShaderDto obj = (ShaderDto)target;
		    return propertyCode switch
		    {
		        Inputs => obj.Inputs,
		        Parameters => obj.Parameters,
		        Source => obj.Source,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    ShaderDto obj = (ShaderDto)target;
		    switch (propertyCode)
		    {
		        case Inputs:  obj.Inputs = (Dictionary<string,string>)value;break;
		        case Parameters:  obj.Parameters = (Dictionary<string,ShaderParameterDto>)value;break;
		        case Source:  obj.Source = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
