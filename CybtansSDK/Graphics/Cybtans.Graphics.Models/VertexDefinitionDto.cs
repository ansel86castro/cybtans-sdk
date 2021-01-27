using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class VertexDefinitionDto : IReflectorMetadataProvider
	{
		private static readonly VertexDefinitionDtoAccesor __accesor = new VertexDefinitionDtoAccesor();
		
		public int Size {get; set;}
		
		public List<VertexElementDto> Elements {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class VertexDefinitionDtoAccesor : IReflectorMetadata
	{
		public const int Size = 1;
		public const int Elements = 2;
		private readonly int[] _props = new []
		{
			Size,Elements
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Size => "Size",
		       Elements => "Elements",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Size" => Size,
		        "Elements" => Elements,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Size => typeof(int),
		        Elements => typeof(List<VertexElementDto>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    VertexDefinitionDto obj = (VertexDefinitionDto)target;
		    return propertyCode switch
		    {
		        Size => obj.Size,
		        Elements => obj.Elements,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    VertexDefinitionDto obj = (VertexDefinitionDto)target;
		    switch (propertyCode)
		    {
		        case Size:  obj.Size = (int)value;break;
		        case Elements:  obj.Elements = (List<VertexElementDto>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
