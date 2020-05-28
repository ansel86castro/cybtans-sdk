using System;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Customer.Models;

namespace Customer.Clients
{
	public interface ICustomerService
	{
		
		[Get("/api/customer")]
		Task<GetCustomersResponse> GetCustomers();
	
	}

}
