using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Tests.Models
{
	public partial class SoftDeleteOrderDto : IReflectorMetadataProvider
	{
		private static readonly SoftDeleteOrderDtoAccesor __accesor = new SoftDeleteOrderDtoAccesor();
		
		public string Name {get; set;}
		
		public bool IsDeleted {get; set;}
		
		public List<SoftDeleteOrderItemDto> Items {get; set;}
		
		public Guid Id {get; set;}
		
		public DateTime? CreateDate {get; set;}
		
		public DateTime? UpdateDate {get; set;}
		
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
		public const int Id = 4;
		public const int CreateDate = 5;
		public const int UpdateDate = 6;
		private readonly int[] _props = new []
		{
			Name,IsDeleted,Items,Id,CreateDate,UpdateDate
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       IsDeleted => "IsDeleted",
		       Items => "Items",
		       Id => "Id",
		       CreateDate => "CreateDate",
		       UpdateDate => "UpdateDate",
		
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
		        "Id" => Id,
		        "CreateDate" => CreateDate,
		        "UpdateDate" => UpdateDate,
		
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
		        Id => typeof(Guid),
		        CreateDate => typeof(DateTime?),
		        UpdateDate => typeof(DateTime?),
		
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
		        Id => obj.Id,
		        CreateDate => obj.CreateDate,
		        UpdateDate => obj.UpdateDate,
		
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
		        case Id:  obj.Id = (Guid)value;break;
		        case CreateDate:  obj.CreateDate = (DateTime?)value;break;
		        case UpdateDate:  obj.UpdateDate = (DateTime?)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
