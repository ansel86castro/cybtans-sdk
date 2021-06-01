
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System.Threading.Tasks;

using mds = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
	public interface ISoftDeleteOrderService 
	{
		
		/// <summary>
		/// Returns a collection of SoftDeleteOrderDto
		/// </summary>
		Task<mds::GetAllSoftDeleteOrderResponse> GetAll(mds::GetAllRequest request);
		
		
		/// <summary>
		/// Returns one SoftDeleteOrderDto by Id
		/// </summary>
		Task<mds::SoftDeleteOrderDto> Get(mds::GetSoftDeleteOrderRequest request);
		
		
		/// <summary>
		/// Creates one SoftDeleteOrderDto
		/// </summary>
		Task<mds::SoftDeleteOrderDto> Create(mds::CreateSoftDeleteOrderRequest request);
		
		
		/// <summary>
		/// Updates one SoftDeleteOrderDto by Id
		/// </summary>
		Task<mds::SoftDeleteOrderDto> Update(mds::UpdateSoftDeleteOrderRequest request);
		
		
		/// <summary>
		/// Deletes one SoftDeleteOrderDto by Id
		/// </summary>
		Task Delete(mds::DeleteSoftDeleteOrderRequest request);
		
	}

}
