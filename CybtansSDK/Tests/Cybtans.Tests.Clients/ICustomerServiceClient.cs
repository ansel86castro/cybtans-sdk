
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System.Threading.Tasks;
using mds = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	public interface ICustomerServiceClient
	{
		
		/// <summary>
		/// Returns a collection of CustomerDto
		/// </summary>
		Task<mds::GetAllCustomerResponse> GetAll(mds::GetAllRequest request = null);
		
		/// <summary>
		/// Returns one CustomerDto by Id
		/// </summary>
		Task<mds::CustomerDto> Get(mds::GetCustomerRequest request);
		
		/// <summary>
		/// Creates one CustomerDto
		/// </summary>
		Task<mds::CustomerDto> Create(mds::CreateCustomerRequest request);
		
		/// <summary>
		/// Updates one CustomerDto by Id
		/// </summary>
		Task<mds::CustomerDto> Update(mds::UpdateCustomerRequest request);
		
		/// <summary>
		/// Deletes one CustomerDto by Id
		/// </summary>
		Task Delete(mds::DeleteCustomerRequest request);
	
	}

}
