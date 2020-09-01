using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class UploadStreamResponse : IReflectorMetadataProvider
	{
		private static readonly UploadStreamResponseAccesor __accesor = new UploadStreamResponseAccesor();
		
		public string M5checksum {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator UploadStreamResponse(string m5Checksum)
		{
			return new UploadStreamResponse { M5checksum = m5Checksum };
		}
	}
	
	
	public sealed class UploadStreamResponseAccesor : IReflectorMetadata
	{
		public const int M5checksum = 1;
		private readonly int[] _props = new []
		{
			M5checksum
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       M5checksum => "M5checksum",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "M5checksum" => M5checksum,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        M5checksum => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UploadStreamResponse obj = (UploadStreamResponse)target;
		    return propertyCode switch
		    {
		        M5checksum => obj.M5checksum,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UploadStreamResponse obj = (UploadStreamResponse)target;
		    switch (propertyCode)
		    {
		        case M5checksum:  obj.M5checksum = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
