using System;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Catalog.Models;

namespace Catalog.Clients
{
	public interface ICatalogService
	{
		[Headers("Authorization: Bearer")]
		[Get("/api/catalog")]
		Task<GetProductListResponse> GetProducts();
		
		[Get("/api/catalog/{request.Id}")]
		Task<Product> GetProduct(GetProductRequest request);
		
		[Post("/api/catalog")]
		Task<Product> CreateProduct([Body(buffered: true)]Product request);
		
		[Put("/api/catalog/{request.Id}")]
		Task<Product> UpdateProduct([Body(buffered: true)]UpdateProductRequest request);
		
		[Delete("/api/catalog/{request.Id}")]
		Task DeleteProduct(DeleteRequest request);
	
	}

}
