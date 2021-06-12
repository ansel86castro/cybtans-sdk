
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
	public class ClientDto : IReflectorMetadataProvider
	{
		private static readonly ClientDtoAccesor __accesor = new ClientDtoAccesor();
		
		public Guid Id {get; set;}
		
		public string Name {get; set;}
		
		public int ClientTypeId {get; set;}
		
		public int? ClientStatusId {get; set;}
		
		public DateTime CreatedAt {get; set;}
		
		public int CreatorId {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	
		#region ClientDto  Accesor
		public sealed class ClientDtoAccesor : IReflectorMetadata
		{
			public const int Id = 1;
			public const int Name = 2;
			public const int ClientTypeId = 3;
			public const int ClientStatusId = 4;
			public const int CreatedAt = 5;
			public const int CreatorId = 6;
			private readonly int[] _props = new []
			{
				Id,Name,ClientTypeId,ClientStatusId,CreatedAt,CreatorId
			};
			
			public int[] GetPropertyCodes() => _props;
			
			public string GetPropertyName(int propertyCode)
			{
			    return propertyCode switch
			    {
			       Id => "Id",
			       Name => "Name",
			       ClientTypeId => "ClientTypeId",
			       ClientStatusId => "ClientStatusId",
			       CreatedAt => "CreatedAt",
			       CreatorId => "CreatorId",
			
			        _ => throw new InvalidOperationException("property code not supported"),
			    };
			}
			
			public int GetPropertyCode(string propertyName)
			{
			    return propertyName switch
			    {
			        "Id" => Id,
			        "Name" => Name,
			        "ClientTypeId" => ClientTypeId,
			        "ClientStatusId" => ClientStatusId,
			        "CreatedAt" => CreatedAt,
			        "CreatorId" => CreatorId,
			
			        _ => -1,
			    };
			}
			
			public Type GetPropertyType(int propertyCode)
			{
			    return propertyCode switch
			    {
			        Id => typeof(Guid),
			        Name => typeof(string),
			        ClientTypeId => typeof(int),
			        ClientStatusId => typeof(int?),
			        CreatedAt => typeof(DateTime),
			        CreatorId => typeof(int),
			
			        _ => throw new InvalidOperationException("property code not supported"),
			    };
			}
			       
			public object GetValue(object target, int propertyCode)
			{
			    ClientDto obj = (ClientDto)target;
			    return propertyCode switch
			    {
			        Id => obj.Id,
			        Name => obj.Name,
			        ClientTypeId => obj.ClientTypeId,
			        ClientStatusId => obj.ClientStatusId,
			        CreatedAt => obj.CreatedAt,
			        CreatorId => obj.CreatorId,
			
			        _ => throw new InvalidOperationException("property code not supported"),
			    };
			}
			
			public void SetValue(object target, int propertyCode, object value)
			{
			    ClientDto obj = (ClientDto)target;
			    switch (propertyCode)
			    {
			        case Id:  obj.Id = (Guid)value;break;
			        case Name:  obj.Name = (string)value;break;
			        case ClientTypeId:  obj.ClientTypeId = (int)value;break;
			        case ClientStatusId:  obj.ClientStatusId = (int?)value;break;
			        case CreatedAt:  obj.CreatedAt = (DateTime)value;break;
			        case CreatorId:  obj.CreatorId = (int)value;break;
			
			        default: throw new InvalidOperationException("property code not supported");
			    }
			}
		
		}
		#endregion
	
	
	}

}
