using System;
using System.Threading.Tasks;
using Catalog.Models;
using System.Collections.Generic;

namespace Catalog.Services
{
	public abstract partial class CatalogService 
	{
		
		public abstract Task<GetProductListResponse> GetProducts(GetProductListRequest request);
		
		
		public abstract Task<Product> GetProduct(GetProductRequest request);
		
		
		public abstract Task<Product> CreateProduct(Product request);
		
		
		public abstract Task<Product> UpdateProduct(UpdateProductRequest request);
		
		
		public abstract Task DeleteProduct(DeleteRequest request);
		
	}

}
