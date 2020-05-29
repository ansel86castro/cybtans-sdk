using System;
using System.Threading.Tasks;
using Customer.Models;
using System.Collections.Generic;

namespace Customer.Services
{
	public abstract partial class CustomerService 
	{
		
		public abstract Task<GetCustomersResponse> GetCustomers();
		
	}

}
