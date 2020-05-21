using Catalog.Models;
using Catalog.Services.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Catalog.Services
{
    public class CatalogServiceImpl : CatalogService
    {
        private CatalogContext _context;

        public CatalogServiceImpl(CatalogContext context)
        {
            _context = context;
        }

        public override async Task<Product> CreateProduct(Product request)
        {
            _context.Products.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public override async Task DeleteProduct(DeleteRequest request)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public override async Task<Product> GetProduct(GetProductRequest request)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id);
        }

        public override async Task<GetProductListResponse> GetProducts()
        {
            var items = await _context.Products.AsNoTracking().ToListAsync();            

            return new GetProductListResponse
            {
                Items = items,
                Page = 1,
                TotalPages = 1
            };
        }

        public override async Task<Product> UpdateProduct(UpdateProductRequest request)
        {
            var product = request.Product;
            product.Id = request.Id;
            var entry = _context.Products.Attach(product);
            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return product;
        }
    }
}
