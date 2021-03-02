using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class MeshSkinDto : IReflectorMetadataProvider
	{
		private static readonly MeshSkinDtoAccesor __accesor = new MeshSkinDtoAccesor();
		
		public Guid Id {get; set;}
		
		public List<Guid> Bones {get; set;}
		
		public Guid Mesh {get; set;}
		
		public List<float> BindShapeMatrix {get; set;}
		
		public List<float> BoneBindingMatrices {get; set;}
		
		public List<MeshLayerBonesDto> LayerBones {get; set;}
		
		public FrameDto RootBone {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class MeshSkinDtoAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Bones = 2;
		public const int Mesh = 3;
		public const int BindShapeMatrix = 4;
		public const int BoneBindingMatrices = 5;
		public const int LayerBones = 6;
		public const int RootBone = 7;
		private readonly int[] _props = new []
		{
			Id,Bones,Mesh,BindShapeMatrix,BoneBindingMatrices,LayerBones,RootBone
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Bones => "Bones",
		       Mesh => "Mesh",
		       BindShapeMatrix => "BindShapeMatrix",
		       BoneBindingMatrices => "BoneBindingMatrices",
		       LayerBones => "LayerBones",
		       RootBone => "RootBone",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Bones" => Bones,
		        "Mesh" => Mesh,
		        "BindShapeMatrix" => BindShapeMatrix,
		        "BoneBindingMatrices" => BoneBindingMatrices,
		        "LayerBones" => LayerBones,
		        "RootBone" => RootBone,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(Guid),
		        Bones => typeof(List<Guid>),
		        Mesh => typeof(Guid),
		        BindShapeMatrix => typeof(List<float>),
		        BoneBindingMatrices => typeof(List<float>),
		        LayerBones => typeof(List<MeshLayerBonesDto>),
		        RootBone => typeof(FrameDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    MeshSkinDto obj = (MeshSkinDto)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Bones => obj.Bones,
		        Mesh => obj.Mesh,
		        BindShapeMatrix => obj.BindShapeMatrix,
		        BoneBindingMatrices => obj.BoneBindingMatrices,
		        LayerBones => obj.LayerBones,
		        RootBone => obj.RootBone,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    MeshSkinDto obj = (MeshSkinDto)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		        case Bones:  obj.Bones = (List<Guid>)value;break;
		        case Mesh:  obj.Mesh = (Guid)value;break;
		        case BindShapeMatrix:  obj.BindShapeMatrix = (List<float>)value;break;
		        case BoneBindingMatrices:  obj.BoneBindingMatrices = (List<float>)value;break;
		        case LayerBones:  obj.LayerBones = (List<MeshLayerBonesDto>)value;break;
		        case RootBone:  obj.RootBone = (FrameDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
