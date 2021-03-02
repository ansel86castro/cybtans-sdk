using System;
using Cybtans.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Graphics.Models
{
	public partial class EffectDto : IReflectorMetadataProvider
	{
		private static readonly EffectDtoAccesor __accesor = new EffectDtoAccesor();
		
		[Required]
		public string Name {get; set;}
		
		[Required]
		public Dictionary<string,PredicateProgramList> Predicates {get; set;}
		
		[Required]
		public Dictionary<string,string> Programs {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class EffectDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int Predicates = 2;
		public const int Programs = 3;
		private readonly int[] _props = new []
		{
			Name,Predicates,Programs
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       Predicates => "Predicates",
		       Programs => "Programs",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Name" => Name,
		        "Predicates" => Predicates,
		        "Programs" => Programs,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Name => typeof(string),
		        Predicates => typeof(Dictionary<string,PredicateProgramList>),
		        Programs => typeof(Dictionary<string,string>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    EffectDto obj = (EffectDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        Predicates => obj.Predicates,
		        Programs => obj.Programs,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    EffectDto obj = (EffectDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case Predicates:  obj.Predicates = (Dictionary<string,PredicateProgramList>)value;break;
		        case Programs:  obj.Programs = (Dictionary<string,string>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
