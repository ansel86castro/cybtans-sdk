using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	public partial interface ICustomerService 
	{
		
		Task<GetAllCustomerResponse> GetAll(GetAllRequest request);
		
		
		Task<CustomerDto> Get(GetCustomerRequest request);
		
		
		Task<CustomerDto> Create(CustomerDto request);
		
		
		Task<CustomerDto> Update(UpdateCustomerRequest request);
		
		
		Task Delete(DeleteCustomerRequest request);
		
	}

}
