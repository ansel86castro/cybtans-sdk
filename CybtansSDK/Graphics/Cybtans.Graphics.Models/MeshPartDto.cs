using System;
using Cybtans.Serialization;

namespace Cybtans.Graphics.Models
{
	public partial class MeshPartDto : IReflectorMetadataProvider
	{
		private static readonly MeshPartDtoAccesor __accesor = new MeshPartDtoAccesor();
		
		public int MaterialIndex {get; set;}
		
		public int LayerId {get; set;}
		
		public int StartIndex {get; set;}
		
		public int PrimitiveCount {get; set;}
		
		public int StartVertex {get; set;}
		
		public int VertexCount {get; set;}
		
		public int IndexCount {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class MeshPartDtoAccesor : IReflectorMetadata
	{
		public const int MaterialIndex = 1;
		public const int LayerId = 2;
		public const int StartIndex = 3;
		public const int PrimitiveCount = 4;
		public const int StartVertex = 5;
		public const int VertexCount = 6;
		public const int IndexCount = 7;
		private readonly int[] _props = new []
		{
			MaterialIndex,LayerId,StartIndex,PrimitiveCount,StartVertex,VertexCount,IndexCount
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       MaterialIndex => "MaterialIndex",
		       LayerId => "LayerId",
		       StartIndex => "StartIndex",
		       PrimitiveCount => "PrimitiveCount",
		       StartVertex => "StartVertex",
		       VertexCount => "VertexCount",
		       IndexCount => "IndexCount",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "MaterialIndex" => MaterialIndex,
		        "LayerId" => LayerId,
		        "StartIndex" => StartIndex,
		        "PrimitiveCount" => PrimitiveCount,
		        "StartVertex" => StartVertex,
		        "VertexCount" => VertexCount,
		        "IndexCount" => IndexCount,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        MaterialIndex => typeof(int),
		        LayerId => typeof(int),
		        StartIndex => typeof(int),
		        PrimitiveCount => typeof(int),
		        StartVertex => typeof(int),
		        VertexCount => typeof(int),
		        IndexCount => typeof(int),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    MeshPartDto obj = (MeshPartDto)target;
		    return propertyCode switch
		    {
		        MaterialIndex => obj.MaterialIndex,
		        LayerId => obj.LayerId,
		        StartIndex => obj.StartIndex,
		        PrimitiveCount => obj.PrimitiveCount,
		        StartVertex => obj.StartVertex,
		        VertexCount => obj.VertexCount,
		        IndexCount => obj.IndexCount,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    MeshPartDto obj = (MeshPartDto)target;
		    switch (propertyCode)
		    {
		        case MaterialIndex:  obj.MaterialIndex = (int)value;break;
		        case LayerId:  obj.LayerId = (int)value;break;
		        case StartIndex:  obj.StartIndex = (int)value;break;
		        case PrimitiveCount:  obj.PrimitiveCount = (int)value;break;
		        case StartVertex:  obj.StartVertex = (int)value;break;
		        case VertexCount:  obj.VertexCount = (int)value;break;
		        case IndexCount:  obj.IndexCount = (int)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
