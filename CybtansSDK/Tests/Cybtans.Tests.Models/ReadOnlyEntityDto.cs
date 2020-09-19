using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class ReadOnlyEntityDto : IReflectorMetadataProvider
	{
		private static readonly ReadOnlyEntityDtoAccesor __accesor = new ReadOnlyEntityDtoAccesor();
		
		public string Name {get; set;}
		
		public DateTime? CreateDate {get; set;}
		
		public DateTime? UpdateDate {get; set;}
		
		public int Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class ReadOnlyEntityDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int CreateDate = 2;
		public const int UpdateDate = 3;
		public const int Id = 4;
		private readonly int[] _props = new []
		{
			Name,CreateDate,UpdateDate,Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       CreateDate => "CreateDate",
		       UpdateDate => "UpdateDate",
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Name" => Name,
		        "CreateDate" => CreateDate,
		        "UpdateDate" => UpdateDate,
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Name => typeof(string),
		        CreateDate => typeof(DateTime?),
		        UpdateDate => typeof(DateTime?),
		        Id => typeof(int),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    ReadOnlyEntityDto obj = (ReadOnlyEntityDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        CreateDate => obj.CreateDate,
		        UpdateDate => obj.UpdateDate,
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    ReadOnlyEntityDto obj = (ReadOnlyEntityDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case CreateDate:  obj.CreateDate = (DateTime?)value;break;
		        case UpdateDate:  obj.UpdateDate = (DateTime?)value;break;
		        case Id:  obj.Id = (int)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
