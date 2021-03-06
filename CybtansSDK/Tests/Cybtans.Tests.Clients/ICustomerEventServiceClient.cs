
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public interface ICustomerEventServiceClient
	{
		
		/// <summary>
		/// Returns a collection of CustomerEventDto
		/// </summary>
		[Headers("Authorization: Bearer")]
		[Get("/api/CustomerEvent")]
		Task<GetAllCustomerEventResponse> GetAll(GetAllRequest request = null);
		
		/// <summary>
		/// Returns one CustomerEventDto by Id
		/// </summary>
		[Headers("Authorization: Bearer")]
		[Get("/api/CustomerEvent/{request.Id}")]
		Task<CustomerEventDto> Get(GetCustomerEventRequest request);
		
		/// <summary>
		/// Creates one CustomerEventDto
		/// </summary>
		[Headers("Authorization: Bearer")]
		[Post("/api/CustomerEvent")]
		Task<CustomerEventDto> Create([Body]CreateCustomerEventRequest request);
		
		/// <summary>
		/// Updates one CustomerEventDto by Id
		/// </summary>
		[Headers("Authorization: Bearer")]
		[Put("/api/CustomerEvent/{request.Id}")]
		Task<CustomerEventDto> Update([Body]UpdateCustomerEventRequest request);
		
		/// <summary>
		/// Deletes one CustomerEventDto by Id
		/// </summary>
		[Headers("Authorization: Bearer")]
		[Delete("/api/CustomerEvent/{request.Id}")]
		Task Delete(DeleteCustomerEventRequest request);
	
	}

}
