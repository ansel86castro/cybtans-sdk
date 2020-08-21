using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Tests.Models
{
	public class SoftDeleteOrderDto : IReflectorMetadataProvider
	{
		private static readonly SoftDeleteOrderDtoAccesor __accesor = new SoftDeleteOrderDtoAccesor();
		
		public string Name {get; set;}
		
		public bool IsDeleted {get; set;}
		
		public List<SoftDeleteOrderItemDto> Items {get; set;}
		
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
	
	
	public sealed class SoftDeleteOrderDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int IsDeleted = 2;
		public const int Items = 3;
		public const int TenantId = 4;
		public const int Id = 5;
		public const int CreateDate = 6;
		public const int UpdateDate = 7;
		public const int Creator = 8;
		private readonly int[] _props = new []
		{
			Name,IsDeleted,Items,TenantId,Id,CreateDate,UpdateDate,Creator
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       IsDeleted => "IsDeleted",
		       Items => "Items",
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
		        "Items" => Items,
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
		        Items => typeof(List<SoftDeleteOrderItemDto>),
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
		    SoftDeleteOrderDto obj = (SoftDeleteOrderDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        IsDeleted => obj.IsDeleted,
		        Items => obj.Items,
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
		    SoftDeleteOrderDto obj = (SoftDeleteOrderDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case IsDeleted:  obj.IsDeleted = (bool)value;break;
		        case Items:  obj.Items = (List<SoftDeleteOrderItemDto>)value;break;
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
