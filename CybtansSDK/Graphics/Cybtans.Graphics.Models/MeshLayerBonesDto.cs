using System;
using Cybtans.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Graphics.Models
{
	public partial class MeshLayerBonesDto : IReflectorMetadataProvider
	{
		private static readonly MeshLayerBonesDtoAccesor __accesor = new MeshLayerBonesDtoAccesor();
		
		public int LayerIndex {get; set;}
		
		[Required]
		public List<int> Bones {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class MeshLayerBonesDtoAccesor : IReflectorMetadata
	{
		public const int LayerIndex = 1;
		public const int Bones = 2;
		private readonly int[] _props = new []
		{
			LayerIndex,Bones
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       LayerIndex => "LayerIndex",
		       Bones => "Bones",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "LayerIndex" => LayerIndex,
		        "Bones" => Bones,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        LayerIndex => typeof(int),
		        Bones => typeof(List<int>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    MeshLayerBonesDto obj = (MeshLayerBonesDto)target;
		    return propertyCode switch
		    {
		        LayerIndex => obj.LayerIndex,
		        Bones => obj.Bones,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    MeshLayerBonesDto obj = (MeshLayerBonesDto)target;
		    switch (propertyCode)
		    {
		        case LayerIndex:  obj.LayerIndex = (int)value;break;
		        case Bones:  obj.Bones = (List<int>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
