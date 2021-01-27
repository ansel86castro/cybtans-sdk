using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class AmbientLightDto : IReflectorMetadataProvider
	{
		private static readonly AmbientLightDtoAccesor __accesor = new AmbientLightDtoAccesor();
		
		public List<float> AmbientColor {get; set;}
		
		public List<float> SkyColor {get; set;}
		
		public List<float> GroundColor {get; set;}
		
		public float Intensity {get; set;}
		
		public List<float> NorthPole {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class AmbientLightDtoAccesor : IReflectorMetadata
	{
		public const int AmbientColor = 1;
		public const int SkyColor = 2;
		public const int GroundColor = 3;
		public const int Intensity = 4;
		public const int NorthPole = 5;
		private readonly int[] _props = new []
		{
			AmbientColor,SkyColor,GroundColor,Intensity,NorthPole
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       AmbientColor => "AmbientColor",
		       SkyColor => "SkyColor",
		       GroundColor => "GroundColor",
		       Intensity => "Intensity",
		       NorthPole => "NorthPole",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "AmbientColor" => AmbientColor,
		        "SkyColor" => SkyColor,
		        "GroundColor" => GroundColor,
		        "Intensity" => Intensity,
		        "NorthPole" => NorthPole,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        AmbientColor => typeof(List<float>),
		        SkyColor => typeof(List<float>),
		        GroundColor => typeof(List<float>),
		        Intensity => typeof(float),
		        NorthPole => typeof(List<float>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    AmbientLightDto obj = (AmbientLightDto)target;
		    return propertyCode switch
		    {
		        AmbientColor => obj.AmbientColor,
		        SkyColor => obj.SkyColor,
		        GroundColor => obj.GroundColor,
		        Intensity => obj.Intensity,
		        NorthPole => obj.NorthPole,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    AmbientLightDto obj = (AmbientLightDto)target;
		    switch (propertyCode)
		    {
		        case AmbientColor:  obj.AmbientColor = (List<float>)value;break;
		        case SkyColor:  obj.SkyColor = (List<float>)value;break;
		        case GroundColor:  obj.GroundColor = (List<float>)value;break;
		        case Intensity:  obj.Intensity = (float)value;break;
		        case NorthPole:  obj.NorthPole = (List<float>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
