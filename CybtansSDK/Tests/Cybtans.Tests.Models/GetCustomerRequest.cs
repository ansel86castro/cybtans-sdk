using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class GetCustomerRequest : IReflectorMetadataProvider
	{
		private static readonly GetCustomerRequestAccesor __accesor = new GetCustomerRequestAccesor();
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator GetCustomerRequest(Guid id)
		{
			return new GetCustomerRequest { Id = id };
		}
	}
	
	
	public sealed class GetCustomerRequestAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		private readonly int[] _props = new []
		{
			Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(Guid),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    GetCustomerRequest obj = (GetCustomerRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    GetCustomerRequest obj = (GetCustomerRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
