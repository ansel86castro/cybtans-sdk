using System;
using Cybtans.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Graphics.Models
{
	public partial class FrameDto : IReflectorMetadataProvider
	{
		private static readonly FrameDtoAccesor __accesor = new FrameDtoAccesor();
		
		public Guid Id {get; set;}
		
		public string Name {get; set;}
		
		[Required]
		public List<float> LocalTransform {get; set;}
		
		[Required]
		public List<float> BindParentTransform {get; set;}
		
		[Required]
		public List<float> BindAffectorTransform {get; set;}
		
		[Required]
		public List<float> WorldTransform {get; set;}
		
		public Guid? ParentId {get; set;}
		
		public FrameType Type {get; set;}
		
		public float Range {get; set;}
		
		public List<FrameDto> Childrens {get; set;}
		
		public Guid? BindTargetId {get; set;}
		
		public string Tag {get; set;}
		
		public FrameComponentDto Component {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class FrameDtoAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Name = 2;
		public const int LocalTransform = 3;
		public const int BindParentTransform = 4;
		public const int BindAffectorTransform = 5;
		public const int WorldTransform = 6;
		public const int ParentId = 7;
		public const int Type = 8;
		public const int Range = 9;
		public const int Childrens = 10;
		public const int BindTargetId = 11;
		public const int Tag = 12;
		public const int Component = 13;
		private readonly int[] _props = new []
		{
			Id,Name,LocalTransform,BindParentTransform,BindAffectorTransform,WorldTransform,ParentId,Type,Range,Childrens,BindTargetId,Tag,Component
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Name => "Name",
		       LocalTransform => "LocalTransform",
		       BindParentTransform => "BindParentTransform",
		       BindAffectorTransform => "BindAffectorTransform",
		       WorldTransform => "WorldTransform",
		       ParentId => "ParentId",
		       Type => "Type",
		       Range => "Range",
		       Childrens => "Childrens",
		       BindTargetId => "BindTargetId",
		       Tag => "Tag",
		       Component => "Component",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Name" => Name,
		        "LocalTransform" => LocalTransform,
		        "BindParentTransform" => BindParentTransform,
		        "BindAffectorTransform" => BindAffectorTransform,
		        "WorldTransform" => WorldTransform,
		        "ParentId" => ParentId,
		        "Type" => Type,
		        "Range" => Range,
		        "Childrens" => Childrens,
		        "BindTargetId" => BindTargetId,
		        "Tag" => Tag,
		        "Component" => Component,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(Guid),
		        Name => typeof(string),
		        LocalTransform => typeof(List<float>),
		        BindParentTransform => typeof(List<float>),
		        BindAffectorTransform => typeof(List<float>),
		        WorldTransform => typeof(List<float>),
		        ParentId => typeof(Guid?),
		        Type => typeof(FrameType),
		        Range => typeof(float),
		        Childrens => typeof(List<FrameDto>),
		        BindTargetId => typeof(Guid?),
		        Tag => typeof(string),
		        Component => typeof(FrameComponentDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    FrameDto obj = (FrameDto)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Name => obj.Name,
		        LocalTransform => obj.LocalTransform,
		        BindParentTransform => obj.BindParentTransform,
		        BindAffectorTransform => obj.BindAffectorTransform,
		        WorldTransform => obj.WorldTransform,
		        ParentId => obj.ParentId,
		        Type => obj.Type,
		        Range => obj.Range,
		        Childrens => obj.Childrens,
		        BindTargetId => obj.BindTargetId,
		        Tag => obj.Tag,
		        Component => obj.Component,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    FrameDto obj = (FrameDto)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		        case Name:  obj.Name = (string)value;break;
		        case LocalTransform:  obj.LocalTransform = (List<float>)value;break;
		        case BindParentTransform:  obj.BindParentTransform = (List<float>)value;break;
		        case BindAffectorTransform:  obj.BindAffectorTransform = (List<float>)value;break;
		        case WorldTransform:  obj.WorldTransform = (List<float>)value;break;
		        case ParentId:  obj.ParentId = (Guid?)value;break;
		        case Type:  obj.Type = (FrameType)value;break;
		        case Range:  obj.Range = (float)value;break;
		        case Childrens:  obj.Childrens = (List<FrameDto>)value;break;
		        case BindTargetId:  obj.BindTargetId = (Guid?)value;break;
		        case Tag:  obj.Tag = (string)value;break;
		        case Component:  obj.Component = (FrameComponentDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
