using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class CustomerEventDto : IReflectorMetadataProvider
	{
		private static readonly CustomerEventDtoAccesor __accesor = new CustomerEventDtoAccesor();
		
		public string FullName {get; set;}
		
		public Guid? CustomerProfileId {get; set;}
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class CustomerEventDtoAccesor : IReflectorMetadata
	{
		public const int FullName = 1;
		public const int CustomerProfileId = 2;
		public const int Id = 3;
		private readonly int[] _props = new []
		{
			FullName,CustomerProfileId,Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       FullName => "FullName",
		       CustomerProfileId => "CustomerProfileId",
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "FullName" => FullName,
		        "CustomerProfileId" => CustomerProfileId,
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        FullName => typeof(string),
		        CustomerProfileId => typeof(Guid?),
		        Id => typeof(Guid),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    CustomerEventDto obj = (CustomerEventDto)target;
		    return propertyCode switch
		    {
		        FullName => obj.FullName,
		        CustomerProfileId => obj.CustomerProfileId,
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    CustomerEventDto obj = (CustomerEventDto)target;
		    switch (propertyCode)
		    {
		        case FullName:  obj.FullName = (string)value;break;
		        case CustomerProfileId:  obj.CustomerProfileId = (Guid?)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
