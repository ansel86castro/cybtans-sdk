
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System;
using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public interface IReadOnlyEntityServiceClient
	{
		
		[Headers("Authorization: Bearer")]
		[Get("/api/ReadOnlyEntity")]
		Task<GetAllReadOnlyEntityResponse> GetAll(GetAllRequest request = null);
		
		[Headers("Authorization: Bearer")]
		[Get("/api/ReadOnlyEntity/{request.Id}")]
		Task<ReadOnlyEntityDto> Get(GetReadOnlyEntityRequest request);
	
	}

}
