using System;
using Cybtans.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Customer.Models
{
	public partial class Customer : IReflectorMetadataProvider
	{
		private static readonly CustomerAccesor __accesor = new CustomerAccesor();
		
		public Guid Id {get; set;}
		
		[Required]
		public string Name {get; set;}
		
		public string Description {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class CustomerAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Name = 2;
		public const int Description = 3;
		private readonly int[] _props = new []
		{
			Id,Name,Description
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Name => "Name",
		       Description => "Description",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Name" => Name,
		        "Description" => Description,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(Guid),
		        Name => typeof(string),
		        Description => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    Customer obj = (Customer)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Name => obj.Name,
		        Description => obj.Description,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    Customer obj = (Customer)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		        case Name:  obj.Name = (string)value;break;
		        case Description:  obj.Description = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
