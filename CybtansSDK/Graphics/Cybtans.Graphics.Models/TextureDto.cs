using System;
using Cybtans.Serialization;

namespace Cybtans.Graphics.Models
{
	public partial class TextureDto : IReflectorMetadataProvider
	{
		private static readonly TextureDtoAccesor __accesor = new TextureDtoAccesor();
		
		public string Filename {get; set;}
		
		public TextureType Type {get; set;}
		
		public string Format {get; set;}
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class TextureDtoAccesor : IReflectorMetadata
	{
		public const int Filename = 1;
		public const int Type = 2;
		public const int Format = 3;
		public const int Id = 4;
		private readonly int[] _props = new []
		{
			Filename,Type,Format,Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Filename => "Filename",
		       Type => "Type",
		       Format => "Format",
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Filename" => Filename,
		        "Type" => Type,
		        "Format" => Format,
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Filename => typeof(string),
		        Type => typeof(TextureType),
		        Format => typeof(string),
		        Id => typeof(Guid),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    TextureDto obj = (TextureDto)target;
		    return propertyCode switch
		    {
		        Filename => obj.Filename,
		        Type => obj.Type,
		        Format => obj.Format,
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    TextureDto obj = (TextureDto)target;
		    switch (propertyCode)
		    {
		        case Filename:  obj.Filename = (string)value;break;
		        case Type:  obj.Type = (TextureType)value;break;
		        case Format:  obj.Format = (string)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
