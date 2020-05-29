using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Customer.Services;
using Customer.Models;

namespace Customer.RestApi.Controllers
{
	[Route("api/customer")]
	[ApiController]
	public class CustomerServiceController : ControllerBase
	{
		private readonly CustomerService _service;
		
		public CustomerServiceController(CustomerService service)
		{
			_service = service;
		}
		
		[HttpGet]
		public async Task<GetCustomersResponse> GetCustomers()
		{
			return await _service.GetCustomers();
		}
	}

}
