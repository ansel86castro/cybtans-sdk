using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class UploadStreamByIdRequest : IReflectorMetadataProvider
	{
		private static readonly UploadStreamByIdRequestAccesor __accesor = new UploadStreamByIdRequestAccesor();
		
		public string Id {get; set;}
		
		public System.IO.Stream Data {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class UploadStreamByIdRequestAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Data = 2;
		private readonly int[] _props = new []
		{
			Id,Data
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Data => "Data",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Data" => Data,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(string),
		        Data => typeof(System.IO.Stream),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UploadStreamByIdRequest obj = (UploadStreamByIdRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Data => obj.Data,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UploadStreamByIdRequest obj = (UploadStreamByIdRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (string)value;break;
		        case Data:  obj.Data = (System.IO.Stream)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
