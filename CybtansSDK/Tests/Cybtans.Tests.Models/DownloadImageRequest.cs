using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class DownloadImageRequest : IReflectorMetadataProvider
	{
		private static readonly DownloadImageRequestAccesor __accesor = new DownloadImageRequestAccesor();
		
		public string Name {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator DownloadImageRequest(string name)
		{
			return new DownloadImageRequest { Name = name };
		}
	}
	
	
	public sealed class DownloadImageRequestAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		private readonly int[] _props = new []
		{
			Name
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Name" => Name,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Name => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    DownloadImageRequest obj = (DownloadImageRequest)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    DownloadImageRequest obj = (DownloadImageRequest)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
