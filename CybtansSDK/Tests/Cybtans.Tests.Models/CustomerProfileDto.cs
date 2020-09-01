using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class CustomerProfileDto : IReflectorMetadataProvider
	{
		private static readonly CustomerProfileDtoAccesor __accesor = new CustomerProfileDtoAccesor();
		
		public string Name {get; set;}
		
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
	
	
	public sealed class CustomerProfileDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int TenantId = 2;
		public const int Id = 3;
		public const int CreateDate = 4;
		public const int UpdateDate = 5;
		public const int Creator = 6;
		private readonly int[] _props = new []
		{
			Name,TenantId,Id,CreateDate,UpdateDate,Creator
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
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
		    CustomerProfileDto obj = (CustomerProfileDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
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
		    CustomerProfileDto obj = (CustomerProfileDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
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
