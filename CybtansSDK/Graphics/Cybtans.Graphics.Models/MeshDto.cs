using System;
using Cybtans.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Graphics.Models
{
	public partial class MeshDto : IReflectorMetadataProvider
	{
		private static readonly MeshDtoAccesor __accesor = new MeshDtoAccesor();
		
		public Guid Id {get; set;}
		
		public List<string> MaterialSlots {get; set;}
		
		public List<int> Adjacency {get; set;}
		
		public int VertexCount {get; set;}
		
		public int FaceCount {get; set;}
		
		[Required]
		public VertexDefinitionDto VertexDeclaration {get; set;}
		
		public byte[] VertexBuffer {get; set;}
		
		public byte[] IndexBuffer {get; set;}
		
		public MeshPrimitive Primitive {get; set;}
		
		[Required]
		public List<MeshPartDto> Layers {get; set;}
		
		public string Name {get; set;}
		
		public bool SixteenBitsIndices {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class MeshDtoAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int MaterialSlots = 2;
		public const int Adjacency = 3;
		public const int VertexCount = 4;
		public const int FaceCount = 5;
		public const int VertexDeclaration = 6;
		public const int VertexBuffer = 7;
		public const int IndexBuffer = 8;
		public const int Primitive = 9;
		public const int Layers = 10;
		public const int Name = 11;
		public const int SixteenBitsIndices = 12;
		private readonly int[] _props = new []
		{
			Id,MaterialSlots,Adjacency,VertexCount,FaceCount,VertexDeclaration,VertexBuffer,IndexBuffer,Primitive,Layers,Name,SixteenBitsIndices
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       MaterialSlots => "MaterialSlots",
		       Adjacency => "Adjacency",
		       VertexCount => "VertexCount",
		       FaceCount => "FaceCount",
		       VertexDeclaration => "VertexDeclaration",
		       VertexBuffer => "VertexBuffer",
		       IndexBuffer => "IndexBuffer",
		       Primitive => "Primitive",
		       Layers => "Layers",
		       Name => "Name",
		       SixteenBitsIndices => "SixteenBitsIndices",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "MaterialSlots" => MaterialSlots,
		        "Adjacency" => Adjacency,
		        "VertexCount" => VertexCount,
		        "FaceCount" => FaceCount,
		        "VertexDeclaration" => VertexDeclaration,
		        "VertexBuffer" => VertexBuffer,
		        "IndexBuffer" => IndexBuffer,
		        "Primitive" => Primitive,
		        "Layers" => Layers,
		        "Name" => Name,
		        "SixteenBitsIndices" => SixteenBitsIndices,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(Guid),
		        MaterialSlots => typeof(List<string>),
		        Adjacency => typeof(List<int>),
		        VertexCount => typeof(int),
		        FaceCount => typeof(int),
		        VertexDeclaration => typeof(VertexDefinitionDto),
		        VertexBuffer => typeof(byte[]),
		        IndexBuffer => typeof(byte[]),
		        Primitive => typeof(MeshPrimitive),
		        Layers => typeof(List<MeshPartDto>),
		        Name => typeof(string),
		        SixteenBitsIndices => typeof(bool),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    MeshDto obj = (MeshDto)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        MaterialSlots => obj.MaterialSlots,
		        Adjacency => obj.Adjacency,
		        VertexCount => obj.VertexCount,
		        FaceCount => obj.FaceCount,
		        VertexDeclaration => obj.VertexDeclaration,
		        VertexBuffer => obj.VertexBuffer,
		        IndexBuffer => obj.IndexBuffer,
		        Primitive => obj.Primitive,
		        Layers => obj.Layers,
		        Name => obj.Name,
		        SixteenBitsIndices => obj.SixteenBitsIndices,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    MeshDto obj = (MeshDto)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		        case MaterialSlots:  obj.MaterialSlots = (List<string>)value;break;
		        case Adjacency:  obj.Adjacency = (List<int>)value;break;
		        case VertexCount:  obj.VertexCount = (int)value;break;
		        case FaceCount:  obj.FaceCount = (int)value;break;
		        case VertexDeclaration:  obj.VertexDeclaration = (VertexDefinitionDto)value;break;
		        case VertexBuffer:  obj.VertexBuffer = (byte[])value;break;
		        case IndexBuffer:  obj.IndexBuffer = (byte[])value;break;
		        case Primitive:  obj.Primitive = (MeshPrimitive)value;break;
		        case Layers:  obj.Layers = (List<MeshPartDto>)value;break;
		        case Name:  obj.Name = (string)value;break;
		        case SixteenBitsIndices:  obj.SixteenBitsIndices = (bool)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
