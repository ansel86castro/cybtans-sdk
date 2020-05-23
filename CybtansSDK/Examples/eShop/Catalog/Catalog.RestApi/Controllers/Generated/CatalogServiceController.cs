using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Catalog.Services;
using Catalog.Models;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.RestApi.Controllers
{	
	[Route("api/catalog")]
	[ApiController]	
	public class CatalogServiceController : ControllerBase
	{
		private readonly CatalogService _service;
		
		public CatalogServiceController(CatalogService service)
		{
			_service = service;
		}
		
		[HttpGet]
		public async Task<GetProductListResponse> GetProducts()
		{
			return await _service.GetProducts();
		}
		
		[HttpGet("{id}")]
		public async Task<Product> GetProduct(int id, [FromQuery]GetProductRequest __request)
		{
			__request.Id = id;
			return await _service.GetProduct(__request);
		}
		
		[HttpPost]
		public async Task<Product> CreateProduct([FromBody]Product __request)
		{
			return await _service.CreateProduct(__request);
		}
		
		[HttpPut("{id}")]
		public async Task<Product> UpdateProduct(int id, [FromBody]UpdateProductRequest __request)
		{
			__request.Id = id;
			return await _service.UpdateProduct(__request);
		}
		
		[HttpDelete("{id}")]
		public async Task DeleteProduct(int id, [FromQuery]DeleteRequest __request)
		{
			__request.Id = id;
			await _service.DeleteProduct(__request);
		}
	}

}
