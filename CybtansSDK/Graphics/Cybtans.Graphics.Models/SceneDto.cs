using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class SceneDto : IReflectorMetadataProvider
	{
		private static readonly SceneDtoAccesor __accesor = new SceneDtoAccesor();
		
		public Guid Id {get; set;}
		
		public string Name {get; set;}
		
		public float Units {get; set;}
		
		public List<MaterialDto> Materials {get; set;}
		
		public List<CameraDto> Cameras {get; set;}
		
		public List<LightDto> Lights {get; set;}
		
		public List<MeshDto> Meshes {get; set;}
		
		public List<MeshSkinDto> Skins {get; set;}
		
		public List<TextureDto> Textures {get; set;}
		
		public FrameDto Root {get; set;}
		
		public Uomtype UnitOfMeasure {get; set;}
		
		public Guid? CurrentCamera {get; set;}
		
		public AmbientLightDto Ambient {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class SceneDtoAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Name = 2;
		public const int Units = 3;
		public const int Materials = 4;
		public const int Cameras = 5;
		public const int Lights = 6;
		public const int Meshes = 7;
		public const int Skins = 8;
		public const int Textures = 9;
		public const int Root = 10;
		public const int UnitOfMeasure = 11;
		public const int CurrentCamera = 12;
		public const int Ambient = 13;
		private readonly int[] _props = new []
		{
			Id,Name,Units,Materials,Cameras,Lights,Meshes,Skins,Textures,Root,UnitOfMeasure,CurrentCamera,Ambient
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Name => "Name",
		       Units => "Units",
		       Materials => "Materials",
		       Cameras => "Cameras",
		       Lights => "Lights",
		       Meshes => "Meshes",
		       Skins => "Skins",
		       Textures => "Textures",
		       Root => "Root",
		       UnitOfMeasure => "UnitOfMeasure",
		       CurrentCamera => "CurrentCamera",
		       Ambient => "Ambient",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Name" => Name,
		        "Units" => Units,
		        "Materials" => Materials,
		        "Cameras" => Cameras,
		        "Lights" => Lights,
		        "Meshes" => Meshes,
		        "Skins" => Skins,
		        "Textures" => Textures,
		        "Root" => Root,
		        "UnitOfMeasure" => UnitOfMeasure,
		        "CurrentCamera" => CurrentCamera,
		        "Ambient" => Ambient,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(Guid),
		        Name => typeof(string),
		        Units => typeof(float),
		        Materials => typeof(List<MaterialDto>),
		        Cameras => typeof(List<CameraDto>),
		        Lights => typeof(List<LightDto>),
		        Meshes => typeof(List<MeshDto>),
		        Skins => typeof(List<MeshSkinDto>),
		        Textures => typeof(List<TextureDto>),
		        Root => typeof(FrameDto),
		        UnitOfMeasure => typeof(Uomtype),
		        CurrentCamera => typeof(Guid?),
		        Ambient => typeof(AmbientLightDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    SceneDto obj = (SceneDto)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Name => obj.Name,
		        Units => obj.Units,
		        Materials => obj.Materials,
		        Cameras => obj.Cameras,
		        Lights => obj.Lights,
		        Meshes => obj.Meshes,
		        Skins => obj.Skins,
		        Textures => obj.Textures,
		        Root => obj.Root,
		        UnitOfMeasure => obj.UnitOfMeasure,
		        CurrentCamera => obj.CurrentCamera,
		        Ambient => obj.Ambient,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    SceneDto obj = (SceneDto)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		        case Name:  obj.Name = (string)value;break;
		        case Units:  obj.Units = (float)value;break;
		        case Materials:  obj.Materials = (List<MaterialDto>)value;break;
		        case Cameras:  obj.Cameras = (List<CameraDto>)value;break;
		        case Lights:  obj.Lights = (List<LightDto>)value;break;
		        case Meshes:  obj.Meshes = (List<MeshDto>)value;break;
		        case Skins:  obj.Skins = (List<MeshSkinDto>)value;break;
		        case Textures:  obj.Textures = (List<TextureDto>)value;break;
		        case Root:  obj.Root = (FrameDto)value;break;
		        case UnitOfMeasure:  obj.UnitOfMeasure = (Uomtype)value;break;
		        case CurrentCamera:  obj.CurrentCamera = (Guid?)value;break;
		        case Ambient:  obj.Ambient = (AmbientLightDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
