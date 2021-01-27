using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class FrameLightDto : IReflectorMetadataProvider
	{
		private static readonly FrameLightDtoAccesor __accesor = new FrameLightDtoAccesor();
		
		public Guid Light {get; set;}
		
		public List<float> LocalPosition {get; set;}
		
		public List<float> LocalDirection {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class FrameLightDtoAccesor : IReflectorMetadata
	{
		public const int Light = 1;
		public const int LocalPosition = 2;
		public const int LocalDirection = 3;
		private readonly int[] _props = new []
		{
			Light,LocalPosition,LocalDirection
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Light => "Light",
		       LocalPosition => "LocalPosition",
		       LocalDirection => "LocalDirection",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Light" => Light,
		        "LocalPosition" => LocalPosition,
		        "LocalDirection" => LocalDirection,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Light => typeof(Guid),
		        LocalPosition => typeof(List<float>),
		        LocalDirection => typeof(List<float>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    FrameLightDto obj = (FrameLightDto)target;
		    return propertyCode switch
		    {
		        Light => obj.Light,
		        LocalPosition => obj.LocalPosition,
		        LocalDirection => obj.LocalDirection,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    FrameLightDto obj = (FrameLightDto)target;
		    switch (propertyCode)
		    {
		        case Light:  obj.Light = (Guid)value;break;
		        case LocalPosition:  obj.LocalPosition = (List<float>)value;break;
		        case LocalDirection:  obj.LocalDirection = (List<float>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
