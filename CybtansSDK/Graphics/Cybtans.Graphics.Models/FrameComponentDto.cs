using System;
using Cybtans.Serialization;

namespace Cybtans.Graphics.Models
{
	public partial class FrameComponentDto : IReflectorMetadataProvider
	{
		private static readonly FrameComponentDtoAccesor __accesor = new FrameComponentDtoAccesor();
		
		public Guid? Camera {get; set;}
		
		public FrameLightDto Light {get; set;}
		
		public FrameMeshDto Mesh {get; set;}
		
		public FrameMeshSkinDto MeshSkin {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class FrameComponentDtoAccesor : IReflectorMetadata
	{
		public const int Camera = 1;
		public const int Light = 2;
		public const int Mesh = 3;
		public const int MeshSkin = 4;
		private readonly int[] _props = new []
		{
			Camera,Light,Mesh,MeshSkin
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Camera => "Camera",
		       Light => "Light",
		       Mesh => "Mesh",
		       MeshSkin => "MeshSkin",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Camera" => Camera,
		        "Light" => Light,
		        "Mesh" => Mesh,
		        "MeshSkin" => MeshSkin,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Camera => typeof(Guid?),
		        Light => typeof(FrameLightDto),
		        Mesh => typeof(FrameMeshDto),
		        MeshSkin => typeof(FrameMeshSkinDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    FrameComponentDto obj = (FrameComponentDto)target;
		    return propertyCode switch
		    {
		        Camera => obj.Camera,
		        Light => obj.Light,
		        Mesh => obj.Mesh,
		        MeshSkin => obj.MeshSkin,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    FrameComponentDto obj = (FrameComponentDto)target;
		    switch (propertyCode)
		    {
		        case Camera:  obj.Camera = (Guid?)value;break;
		        case Light:  obj.Light = (FrameLightDto)value;break;
		        case Mesh:  obj.Mesh = (FrameMeshDto)value;break;
		        case MeshSkin:  obj.MeshSkin = (FrameMeshSkinDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
