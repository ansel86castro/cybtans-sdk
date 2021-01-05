using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class UploadImageResponse : IReflectorMetadataProvider
	{
		private static readonly UploadImageResponseAccesor __accesor = new UploadImageResponseAccesor();
		
		public string Url {get; set;}
		
		public string M5checksum {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class UploadImageResponseAccesor : IReflectorMetadata
	{
		public const int Url = 1;
		public const int M5checksum = 2;
		private readonly int[] _props = new []
		{
			Url,M5checksum
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Url => "Url",
		       M5checksum => "M5checksum",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Url" => Url,
		        "M5checksum" => M5checksum,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Url => typeof(string),
		        M5checksum => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UploadImageResponse obj = (UploadImageResponse)target;
		    return propertyCode switch
		    {
		        Url => obj.Url,
		        M5checksum => obj.M5checksum,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UploadImageResponse obj = (UploadImageResponse)target;
		    switch (propertyCode)
		    {
		        case Url:  obj.Url = (string)value;break;
		        case M5checksum:  obj.M5checksum = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
