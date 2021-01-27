using System;
using Cybtans.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Graphics.Models
{
	public partial class MaterialDto : IReflectorMetadataProvider
	{
		private static readonly MaterialDtoAccesor __accesor = new MaterialDtoAccesor();
		
		public string Name {get; set;}
		
		[Required]
		public List<float> Diffuse {get; set;}
		
		[Required]
		public List<float> Specular {get; set;}
		
		[Required]
		public List<float> Emissive {get; set;}
		
		public Dictionary<string,Guid> Textures {get; set;}
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class MaterialDtoAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int Diffuse = 2;
		public const int Specular = 3;
		public const int Emissive = 4;
		public const int Textures = 5;
		public const int Id = 6;
		private readonly int[] _props = new []
		{
			Name,Diffuse,Specular,Emissive,Textures,Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       Diffuse => "Diffuse",
		       Specular => "Specular",
		       Emissive => "Emissive",
		       Textures => "Textures",
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Name" => Name,
		        "Diffuse" => Diffuse,
		        "Specular" => Specular,
		        "Emissive" => Emissive,
		        "Textures" => Textures,
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Name => typeof(string),
		        Diffuse => typeof(List<float>),
		        Specular => typeof(List<float>),
		        Emissive => typeof(List<float>),
		        Textures => typeof(Dictionary<string,Guid>),
		        Id => typeof(Guid),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    MaterialDto obj = (MaterialDto)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        Diffuse => obj.Diffuse,
		        Specular => obj.Specular,
		        Emissive => obj.Emissive,
		        Textures => obj.Textures,
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    MaterialDto obj = (MaterialDto)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case Diffuse:  obj.Diffuse = (List<float>)value;break;
		        case Specular:  obj.Specular = (List<float>)value;break;
		        case Emissive:  obj.Emissive = (List<float>)value;break;
		        case Textures:  obj.Textures = (Dictionary<string,Guid>)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
