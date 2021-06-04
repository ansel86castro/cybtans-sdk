
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
	public interface ISoftDeleteOrderServiceClient
	{
		
		/// <summary>
		/// Returns a collection of SoftDeleteOrderDto
		/// </summary>
		[Get("/api/SoftDeleteOrder")]
		Task<GetAllSoftDeleteOrderResponse> GetAll(GetAllRequest request = null);
		
		/// <summary>
		/// Returns one SoftDeleteOrderDto by Id
		/// </summary>
		[Get("/api/SoftDeleteOrder/{request.Id}")]
		Task<SoftDeleteOrderDto> Get(GetSoftDeleteOrderRequest request);
		
		/// <summary>
		/// Creates one SoftDeleteOrderDto
		/// </summary>
		[Post("/api/SoftDeleteOrder")]
		Task<SoftDeleteOrderDto> Create([Body]CreateSoftDeleteOrderRequest request);
		
		/// <summary>
		/// Updates one SoftDeleteOrderDto by Id
		/// </summary>
		[Put("/api/SoftDeleteOrder/{request.Id}")]
		Task<SoftDeleteOrderDto> Update([Body]UpdateSoftDeleteOrderRequest request);
		
		/// <summary>
		/// Deletes one SoftDeleteOrderDto by Id
		/// </summary>
		[Delete("/api/SoftDeleteOrder/{request.Id}")]
		Task Delete(DeleteSoftDeleteOrderRequest request);
	
	}

}
