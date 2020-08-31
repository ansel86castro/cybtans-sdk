using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class SoftDeleteOrderItemDto : IReflectorMetadataProvider
	{
		private static readonly SoftDeleteOrderItemDtoAccesor __accesor = new SoftDeleteOrderItemDtoAccesor();
		
		public string Name {get; set;}
		
		public bool IsDeleted {get; set;}
		
		public Guid SoftDeleteOrderId {get; set;}
		
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
	
	
	public sealed class SoftDeleteOrderItemDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int IsDeleted = 2;
		public const int SoftDeleteOrderId = 3;
		public const int TenantId = 4;
		public const int Id = 5;
		public const int CreateDate = 6;
		public const int UpdateDate = 7;
		public const int Creator = 8;
		private readonly int[] _props = new []
		{
			Name,IsDeleted,SoftDeleteOrderId,TenantId,Id,CreateDate,UpdateDate,Creator
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       IsDeleted => "IsDeleted",
		       SoftDeleteOrderId => "SoftDeleteOrderId",
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
		        "IsDeleted" => IsDeleted,
		        "SoftDeleteOrderId" => SoftDeleteOrderId,
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
		        IsDeleted => typeof(bool),
		        SoftDeleteOrderId => typeof(Guid),
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
		    SoftDeleteOrderItemDto obj = (SoftDeleteOrderItemDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        IsDeleted => obj.IsDeleted,
		        SoftDeleteOrderId => obj.SoftDeleteOrderId,
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
		    SoftDeleteOrderItemDto obj = (SoftDeleteOrderItemDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case IsDeleted:  obj.IsDeleted = (bool)value;break;
		        case SoftDeleteOrderId:  obj.SoftDeleteOrderId = (Guid)value;break;
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
