using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class UploadImageResponse : IReflectorMetadataProvider
	{
		private static readonly UploadImageResponseAccesor __accesor = new UploadImageResponseAccesor();
		
		public string Url {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator UploadImageResponse(string url)
		{
			return new UploadImageResponse { Url = url };
		}
	}
	
	
	public sealed class UploadImageResponseAccesor : IReflectorMetadata
	{
		public const int Url = 1;
		private readonly int[] _props = new []
		{
			Url
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Url => "Url",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Url" => Url,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Url => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UploadImageResponse obj = (UploadImageResponse)target;
		    return propertyCode switch
		    {
		        Url => obj.Url,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UploadImageResponse obj = (UploadImageResponse)target;
		    switch (propertyCode)
		    {
		        case Url:  obj.Url = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
