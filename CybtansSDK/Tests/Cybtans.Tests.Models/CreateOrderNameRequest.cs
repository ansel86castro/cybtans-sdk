
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class CreateOrderNameRequest : IReflectorMetadataProvider
	{
		private static readonly CreateOrderNameRequestAccesor __accesor = new CreateOrderNameRequestAccesor();
		
		public string Name {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		public static implicit operator CreateOrderNameRequest(string name)
		{
			return new CreateOrderNameRequest { Name = name };
		}
	
	
		#region CreateOrderNameRequest  Accesor
		public sealed class CreateOrderNameRequestAccesor : IReflectorMetadata
		{
			public const int Name = 1;
			private readonly int[] _props = new int[]
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
			    CreateOrderNameRequest obj = (CreateOrderNameRequest)target;
			    return propertyCode switch
			    {
			        Name => obj.Name,
			
			        _ => throw new InvalidOperationException("property code not supported"),
			    };
			}
			
			public void SetValue(object target, int propertyCode, object value)
			{
			    CreateOrderNameRequest obj = (CreateOrderNameRequest)target;
			    switch (propertyCode)
			    {
			        case Name:  obj.Name = (string)value;break;
			
			        default: throw new InvalidOperationException("property code not supported");
			    }
			}
		
		}
		#endregion
	
	
	}

}
