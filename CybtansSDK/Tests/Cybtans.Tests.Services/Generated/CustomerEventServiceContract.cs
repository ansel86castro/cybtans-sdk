using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	public interface ICustomerEventService 
	{
		
		Task<GetAllCustomerEventResponse> GetAll(GetAllRequest request);
		
		
		Task<CustomerEventDto> Get(GetCustomerEventRequest request);
		
		
		Task<CustomerEventDto> Create(CustomerEventDto request);
		
		
		Task<CustomerEventDto> Update(UpdateCustomerEventRequest request);
		
		
		Task Delete(DeleteCustomerEventRequest request);
		
	}

}
