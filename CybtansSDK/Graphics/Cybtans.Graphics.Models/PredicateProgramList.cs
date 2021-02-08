using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class PredicateProgramList : IReflectorMetadataProvider
	{
		private static readonly PredicateProgramListAccesor __accesor = new PredicateProgramListAccesor();
		
		public List<PredicateProgramDto> Items {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator PredicateProgramList(List<PredicateProgramDto> items)
		{
			return new PredicateProgramList { Items = items };
		}
	}
	
	
	public sealed class PredicateProgramListAccesor : IReflectorMetadata
	{
		public const int Items = 1;
		private readonly int[] _props = new []
		{
			Items
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Items => "Items",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Items" => Items,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Items => typeof(List<PredicateProgramDto>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    PredicateProgramList obj = (PredicateProgramList)target;
		    return propertyCode switch
		    {
		        Items => obj.Items,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    PredicateProgramList obj = (PredicateProgramList)target;
		    switch (propertyCode)
		    {
		        case Items:  obj.Items = (List<PredicateProgramDto>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
