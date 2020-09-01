using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class CustomerDto : IReflectorMetadataProvider
	{
		private static readonly CustomerDtoAccesor __accesor = new CustomerDtoAccesor();
		
		public string Name {get; set;}
		
		public string FirstLastName {get; set;}
		
		public string SecondLastName {get; set;}
		
		public Guid? CustomerProfileId {get; set;}
		
		public CustomerProfileDto CustomerProfile {get; set;}
		
		public Guid? TenantId {get; set;}
		
		public Guid Id {get; set;}
		
		public DateTime CreateDate {get; set;}
		
		public DateTime? UpdateDate {get; set;}
		
		public string Creator {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class CustomerDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int FirstLastName = 2;
		public const int SecondLastName = 3;
		public const int CustomerProfileId = 4;
		public const int CustomerProfile = 5;
		public const int TenantId = 6;
		public const int Id = 7;
		public const int CreateDate = 8;
		public const int UpdateDate = 9;
		public const int Creator = 10;
		private readonly int[] _props = new []
		{
			Name,FirstLastName,SecondLastName,CustomerProfileId,CustomerProfile,TenantId,Id,CreateDate,UpdateDate,Creator
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       FirstLastName => "FirstLastName",
		       SecondLastName => "SecondLastName",
		       CustomerProfileId => "CustomerProfileId",
		       CustomerProfile => "CustomerProfile",
		       TenantId => "TenantId",
		       Id => "Id",
		       CreateDate => "CreateDate",
		       UpdateDate => "UpdateDate",
		       Creator => "Creator",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Name" => Name,
		        "FirstLastName" => FirstLastName,
		        "SecondLastName" => SecondLastName,
		        "CustomerProfileId" => CustomerProfileId,
		        "CustomerProfile" => CustomerProfile,
		        "TenantId" => TenantId,
		        "Id" => Id,
		        "CreateDate" => CreateDate,
		        "UpdateDate" => UpdateDate,
		        "Creator" => Creator,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Name => typeof(string),
		        FirstLastName => typeof(string),
		        SecondLastName => typeof(string),
		        CustomerProfileId => typeof(Guid?),
		        CustomerProfile => typeof(CustomerProfileDto),
		        TenantId => typeof(Guid?),
		        Id => typeof(Guid),
		        CreateDate => typeof(DateTime),
		        UpdateDate => typeof(DateTime?),
		        Creator => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    CustomerDto obj = (CustomerDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        FirstLastName => obj.FirstLastName,
		        SecondLastName => obj.SecondLastName,
		        CustomerProfileId => obj.CustomerProfileId,
		        CustomerProfile => obj.CustomerProfile,
		        TenantId => obj.TenantId,
		        Id => obj.Id,
		        CreateDate => obj.CreateDate,
		        UpdateDate => obj.UpdateDate,
		        Creator => obj.Creator,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    CustomerDto obj = (CustomerDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case FirstLastName:  obj.FirstLastName = (string)value;break;
		        case SecondLastName:  obj.SecondLastName = (string)value;break;
		        case CustomerProfileId:  obj.CustomerProfileId = (Guid?)value;break;
		        case CustomerProfile:  obj.CustomerProfile = (CustomerProfileDto)value;break;
		        case TenantId:  obj.TenantId = (Guid?)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		        case CreateDate:  obj.CreateDate = (DateTime)value;break;
		        case UpdateDate:  obj.UpdateDate = (DateTime?)value;break;
		        case Creator:  obj.Creator = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
