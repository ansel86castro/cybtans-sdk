
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System.Threading.Tasks;

using models = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
	public interface ICustomerService 
	{
		
		/// <summary>
		/// Returns a collection of CustomerDto
		/// </summary>
		Task<models::GetAllCustomerResponse> GetAll(models::GetAllRequest request);
		
		
		/// <summary>
		/// Returns one CustomerDto by Id
		/// </summary>
		Task<models::CustomerDto> Get(models::GetCustomerRequest request);
		
		
		/// <summary>
		/// Creates one CustomerDto
		/// </summary>
		Task<models::CustomerDto> Create(models::CreateCustomerRequest request);
		
		
		/// <summary>
		/// Updates one CustomerDto by Id
		/// </summary>
		Task<models::CustomerDto> Update(models::UpdateCustomerRequest request);
		
		
		/// <summary>
		/// Deletes one CustomerDto by Id
		/// </summary>
		Task Delete(models::DeleteCustomerRequest request);
		
	}

}