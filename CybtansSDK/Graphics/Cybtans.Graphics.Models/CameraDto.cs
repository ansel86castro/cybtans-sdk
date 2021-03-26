using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Graphics.Models
{
	public partial class CameraDto : IReflectorMetadataProvider
	{
		private static readonly CameraDtoAccesor __accesor = new CameraDtoAccesor();
		
		public ProjectionType ProjType {get; set;}
		
		public string Name {get; set;}
		
		public float NearPlane {get; set;}
		
		public float FarPlane {get; set;}
		
		public float FieldOfView {get; set;}
		
		public float AspectRatio {get; set;}
		
		public float Width {get; set;}
		
		public float Height {get; set;}
		
		public List<float> LocalMatrix {get; set;}
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class CameraDtoAccesor : IReflectorMetadata
	{
		public const int ProjType = 1;
		public const int Name = 2;
		public const int NearPlane = 3;
		public const int FarPlane = 4;
		public const int FieldOfView = 5;
		public const int AspectRatio = 6;
		public const int Width = 7;
		public const int Height = 8;
		public const int LocalMatrix = 9;
		public const int Id = 12;
		private readonly int[] _props = new []
		{
			ProjType,Name,NearPlane,FarPlane,FieldOfView,AspectRatio,Width,Height,LocalMatrix,Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       ProjType => "ProjType",
		       Name => "Name",
		       NearPlane => "NearPlane",
		       FarPlane => "FarPlane",
		       FieldOfView => "FieldOfView",
		       AspectRatio => "AspectRatio",
		       Width => "Width",
		       Height => "Height",
		       LocalMatrix => "LocalMatrix",
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "ProjType" => ProjType,
		        "Name" => Name,
		        "NearPlane" => NearPlane,
		        "FarPlane" => FarPlane,
		        "FieldOfView" => FieldOfView,
		        "AspectRatio" => AspectRatio,
		        "Width" => Width,
		        "Height" => Height,
		        "LocalMatrix" => LocalMatrix,
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        ProjType => typeof(ProjectionType),
		        Name => typeof(string),
		        NearPlane => typeof(float),
		        FarPlane => typeof(float),
		        FieldOfView => typeof(float),
		        AspectRatio => typeof(float),
		        Width => typeof(float),
		        Height => typeof(float),
		        LocalMatrix => typeof(List<float>),
		        Id => typeof(Guid),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    CameraDto obj = (CameraDto)target;
		    return propertyCode switch
		    {
		        ProjType => obj.ProjType,
		        Name => obj.Name,
		        NearPlane => obj.NearPlane,
		        FarPlane => obj.FarPlane,
		        FieldOfView => obj.FieldOfView,
		        AspectRatio => obj.AspectRatio,
		        Width => obj.Width,
		        Height => obj.Height,
		        LocalMatrix => obj.LocalMatrix,
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    CameraDto obj = (CameraDto)target;
		    switch (propertyCode)
		    {
		        case ProjType:  obj.ProjType = (ProjectionType)value;break;
		        case Name:  obj.Name = (string)value;break;
		        case NearPlane:  obj.NearPlane = (float)value;break;
		        case FarPlane:  obj.FarPlane = (float)value;break;
		        case FieldOfView:  obj.FieldOfView = (float)value;break;
		        case AspectRatio:  obj.AspectRatio = (float)value;break;
		        case Width:  obj.Width = (float)value;break;
		        case Height:  obj.Height = (float)value;break;
		        case LocalMatrix:  obj.LocalMatrix = (List<float>)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
