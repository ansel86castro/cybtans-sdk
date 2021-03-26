using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class ProductDto : IReflectorMetadataProvider
	{
		private static readonly ProductDtoAccesor __accesor = new ProductDtoAccesor();
		
		public string Name {get; set;}
		
		public string Model {get; set;}
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class ProductDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int Model = 2;
		public const int Id = 3;
		private readonly int[] _props = new []
		{
			Name,Model,Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       Model => "Model",
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Name" => Name,
		        "Model" => Model,
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Name => typeof(string),
		        Model => typeof(string),
		        Id => typeof(Guid),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    ProductDto obj = (ProductDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        Model => obj.Model,
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    ProductDto obj = (ProductDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case Model:  obj.Model = (string)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
