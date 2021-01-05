using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class OrderStateDto : IReflectorMetadataProvider
	{
		private static readonly OrderStateDtoAccesor __accesor = new OrderStateDtoAccesor();
		
		public string Name {get; set;}
		
		public int Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class OrderStateDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int Id = 2;
		private readonly int[] _props = new []
		{
			Name,Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Name" => Name,
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Name => typeof(string),
		        Id => typeof(int),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    OrderStateDto obj = (OrderStateDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    OrderStateDto obj = (OrderStateDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case Id:  obj.Id = (int)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
