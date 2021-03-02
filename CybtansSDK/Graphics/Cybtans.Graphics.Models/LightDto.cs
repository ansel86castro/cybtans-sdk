using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class LightDto : IReflectorMetadataProvider
	{
		private static readonly LightDtoAccesor __accesor = new LightDtoAccesor();
		
		public List<float> Diffuse {get; set;}
		
		public List<float> Specular {get; set;}
		
		public List<float> Ambient {get; set;}
		
		public List<float> Attenuation {get; set;}
		
		public bool Enable {get; set;}
		
		public float Intensity {get; set;}
		
		public float SpotPower {get; set;}
		
		public LightType Type {get; set;}
		
		public float Range {get; set;}
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class LightDtoAccesor : IReflectorMetadata
	{
		public const int Diffuse = 1;
		public const int Specular = 2;
		public const int Ambient = 3;
		public const int Attenuation = 4;
		public const int Enable = 5;
		public const int Intensity = 6;
		public const int SpotPower = 7;
		public const int Type = 8;
		public const int Range = 9;
		public const int Id = 10;
		private readonly int[] _props = new []
		{
			Diffuse,Specular,Ambient,Attenuation,Enable,Intensity,SpotPower,Type,Range,Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Diffuse => "Diffuse",
		       Specular => "Specular",
		       Ambient => "Ambient",
		       Attenuation => "Attenuation",
		       Enable => "Enable",
		       Intensity => "Intensity",
		       SpotPower => "SpotPower",
		       Type => "Type",
		       Range => "Range",
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Diffuse" => Diffuse,
		        "Specular" => Specular,
		        "Ambient" => Ambient,
		        "Attenuation" => Attenuation,
		        "Enable" => Enable,
		        "Intensity" => Intensity,
		        "SpotPower" => SpotPower,
		        "Type" => Type,
		        "Range" => Range,
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Diffuse => typeof(List<float>),
		        Specular => typeof(List<float>),
		        Ambient => typeof(List<float>),
		        Attenuation => typeof(List<float>),
		        Enable => typeof(bool),
		        Intensity => typeof(float),
		        SpotPower => typeof(float),
		        Type => typeof(LightType),
		        Range => typeof(float),
		        Id => typeof(Guid),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    LightDto obj = (LightDto)target;
		    return propertyCode switch
		    {
		        Diffuse => obj.Diffuse,
		        Specular => obj.Specular,
		        Ambient => obj.Ambient,
		        Attenuation => obj.Attenuation,
		        Enable => obj.Enable,
		        Intensity => obj.Intensity,
		        SpotPower => obj.SpotPower,
		        Type => obj.Type,
		        Range => obj.Range,
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    LightDto obj = (LightDto)target;
		    switch (propertyCode)
		    {
		        case Diffuse:  obj.Diffuse = (List<float>)value;break;
		        case Specular:  obj.Specular = (List<float>)value;break;
		        case Ambient:  obj.Ambient = (List<float>)value;break;
		        case Attenuation:  obj.Attenuation = (List<float>)value;break;
		        case Enable:  obj.Enable = (bool)value;break;
		        case Intensity:  obj.Intensity = (float)value;break;
		        case SpotPower:  obj.SpotPower = (float)value;break;
		        case Type:  obj.Type = (LightType)value;break;
		        case Range:  obj.Range = (float)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
