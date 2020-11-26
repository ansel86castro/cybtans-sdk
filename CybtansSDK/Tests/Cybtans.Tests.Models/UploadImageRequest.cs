using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class UploadImageRequest : IReflectorMetadataProvider
	{
		private static readonly UploadImageRequestAccesor __accesor = new UploadImageRequestAccesor();
		
		public string Name {get; set;}
		
		public int Size {get; set;}
		
		public System.IO.Stream Image {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	internal sealed class UploadImageRequestAccesor : IReflectorMetadata
	{
		public const int Name = 1;
		public const int Size = 2;
		public const int Image = 3;
		private readonly int[] _props = new []
		{
			Name,Size,Image
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Name => "Name",
		       Size => "Size",
		       Image => "Image",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Name" => Name,
		        "Size" => Size,
		        "Image" => Image,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Name => typeof(string),
		        Size => typeof(int),
		        Image => typeof(System.IO.Stream),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UploadImageRequest obj = (UploadImageRequest)target;
		    return propertyCode switch
		    {
		        Name => obj.Name,
		        Size => obj.Size,
		        Image => obj.Image,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UploadImageRequest obj = (UploadImageRequest)target;
		    switch (propertyCode)
		    {
		        case Name:  obj.Name = (string)value;break;
		        case Size:  obj.Size = (int)value;break;
		        case Image:  obj.Image = (System.IO.Stream)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
