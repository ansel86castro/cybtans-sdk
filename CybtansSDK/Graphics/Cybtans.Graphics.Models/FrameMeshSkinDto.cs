using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class FrameMeshSkinDto : IReflectorMetadataProvider
	{
		private static readonly FrameMeshSkinDtoAccesor __accesor = new FrameMeshSkinDtoAccesor();
		
		public Guid MeshSkin {get; set;}
		
		public List<Guid> Materials {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class FrameMeshSkinDtoAccesor : IReflectorMetadata
	{
		public const int MeshSkin = 1;
		public const int Materials = 2;
		private readonly int[] _props = new []
		{
			MeshSkin,Materials
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       MeshSkin => "MeshSkin",
		       Materials => "Materials",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "MeshSkin" => MeshSkin,
		        "Materials" => Materials,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        MeshSkin => typeof(Guid),
		        Materials => typeof(List<Guid>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    FrameMeshSkinDto obj = (FrameMeshSkinDto)target;
		    return propertyCode switch
		    {
		        MeshSkin => obj.MeshSkin,
		        Materials => obj.Materials,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    FrameMeshSkinDto obj = (FrameMeshSkinDto)target;
		    switch (propertyCode)
		    {
		        case MeshSkin:  obj.MeshSkin = (Guid)value;break;
		        case Materials:  obj.Materials = (List<Guid>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
