using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class FrameMeshDto : IReflectorMetadataProvider
	{
		private static readonly FrameMeshDtoAccesor __accesor = new FrameMeshDtoAccesor();
		
		public Guid Mesh {get; set;}
		
		public List<Guid> Materials {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class FrameMeshDtoAccesor : IReflectorMetadata
	{
		public const int Mesh = 1;
		public const int Materials = 2;
		private readonly int[] _props = new []
		{
			Mesh,Materials
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Mesh => "Mesh",
		       Materials => "Materials",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Mesh" => Mesh,
		        "Materials" => Materials,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Mesh => typeof(Guid),
		        Materials => typeof(List<Guid>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    FrameMeshDto obj = (FrameMeshDto)target;
		    return propertyCode switch
		    {
		        Mesh => obj.Mesh,
		        Materials => obj.Materials,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    FrameMeshDto obj = (FrameMeshDto)target;
		    switch (propertyCode)
		    {
		        case Mesh:  obj.Mesh = (Guid)value;break;
		        case Materials:  obj.Materials = (List<Guid>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
