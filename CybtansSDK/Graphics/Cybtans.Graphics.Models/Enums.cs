using System;
using System.ComponentModel;

namespace Cybtans.Graphics.Models
{
	public enum ProjectionType 
	{
		Perspective = 1,
		
		Orthographic = 2,
		
	}
	
	public enum LightType 
	{
		None = 0,
		
		Directional = 1,
		
		Point = 2,
		
		Spot = 3,
		
	}
	
	public enum VertexElementFormat 
	{
		Unused = 0,
		
		Byte = 1,
		
		Ubyte = 2,
		
		Short = 3,
		
		Ushort = 4,
		
		Int = 5,
		
		Uint = 6,
		
		Float = 7,
		
		Double = 8,
		
	}
	
	public enum MeshPrimitive 
	{
		PointList = 1,
		
		LineList = 2,
		
		LineStrip = 3,
		
		TriangleList = 4,
		
		TriangleStrip = 5,
		
		TriangleFan = 6,
		
	}
	
	public enum TextureType 
	{
		None = 0,
		
		Texture2d = 1,
		
		Texture3d = 2,
		
		TextureCube = 3,
		
	}
	
	public enum Uomtype 
	{
		Meters = 0,
		
		Kilometers = 1,
		
		Foot = 2,
		
		Miles = 3,
		
		Centimeters = 4,
		
		Milimeters = 5,
		
		Inches = 6,
		
		Parsec = 7,
		
		LightYear = 8,
		
	}
	
	public enum FrameType 
	{
		Frame = 0,
		
		Bone = 1,
		
		Root = 2,
		
	}

}
