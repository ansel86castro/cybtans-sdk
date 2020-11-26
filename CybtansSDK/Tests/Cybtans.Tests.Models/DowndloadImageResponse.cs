using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class DowndloadImageResponse : IReflectorMetadataProvider
	{
		private static readonly DowndloadImageResponseAccesor __accesor = new DowndloadImageResponseAccesor();
		
		public string FileName {get; set;}
		
		public string ContentType {get; set;}
		
		public System.IO.Stream Image {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	internal sealed class DowndloadImageResponseAccesor : IReflectorMetadata
	{
		public const int FileName = 1;
		public const int ContentType = 2;
		public const int Image = 3;
		private readonly int[] _props = new []
		{
			FileName,ContentType,Image
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       FileName => "FileName",
		       ContentType => "ContentType",
		       Image => "Image",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "FileName" => FileName,
		        "ContentType" => ContentType,
		        "Image" => Image,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        FileName => typeof(string),
		        ContentType => typeof(string),
		        Image => typeof(System.IO.Stream),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    DowndloadImageResponse obj = (DowndloadImageResponse)target;
		    return propertyCode switch
		    {
		        FileName => obj.FileName,
		        ContentType => obj.ContentType,
		        Image => obj.Image,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    DowndloadImageResponse obj = (DowndloadImageResponse)target;
		    switch (propertyCode)
		    {
		        case FileName:  obj.FileName = (string)value;break;
		        case ContentType:  obj.ContentType = (string)value;break;
		        case Image:  obj.Image = (System.IO.Stream)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
