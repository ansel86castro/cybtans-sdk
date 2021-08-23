
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System;
using Cybtans.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Tests.Models
{
	public class ClientRequest : IReflectorMetadataProvider
	{
		private static readonly ClientRequestAccesor __accesor = new ClientRequestAccesor();
		
		[Required]
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		public static implicit operator ClientRequest(Guid id)
		{
			return new ClientRequest { Id = id };
		}
	
	
		#region ClientRequest  Accesor
		public sealed class ClientRequestAccesor : IReflectorMetadata
		{
			public const int Id = 1;
			private readonly int[] _props = new int[]
			{
				Id
			};
			
			public int[] GetPropertyCodes() => _props;
			
			public string GetPropertyName(int propertyCode)
			{
			    return propertyCode switch
			    {
			       Id => "Id",
			
			        _ => throw new InvalidOperationException("property code not supported"),
			    };
			}
			
			public int GetPropertyCode(string propertyName)
			{
			    return propertyName switch
			    {
			        "Id" => Id,
			
			        _ => -1,
			    };
			}
			
			public Type GetPropertyType(int propertyCode)
			{
			    return propertyCode switch
			    {
			        Id => typeof(Guid),
			
			        _ => throw new InvalidOperationException("property code not supported"),
			    };
			}
			       
			public object GetValue(object target, int propertyCode)
			{
			    ClientRequest obj = (ClientRequest)target;
			    return propertyCode switch
			    {
			        Id => obj.Id,
			
			        _ => throw new InvalidOperationException("property code not supported"),
			    };
			}
			
			public void SetValue(object target, int propertyCode, object value)
			{
			    ClientRequest obj = (ClientRequest)target;
			    switch (propertyCode)
			    {
			        case Id:  obj.Id = (Guid)value;break;
			
			        default: throw new InvalidOperationException("property code not supported");
			    }
			}
		
		}
		#endregion
	
	
	}

}