using Catalog.Models;
using Catalog.Services.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Cybtans.Serialization;

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
            request.CreateDate = new DateTime();
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
            var product = await _context.Products
                .Include(x => x.Brand)
                .Include(x=>x.Catalog)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (product == null)
                throw new InvalidOperationException("Object not found");

            return product;
        }

        public override async Task<GetProductListResponse> GetProducts(GetProductListRequest request)
        {            
            var query = _context.Products
                .Include(x=>x.Brand)
                .Include(x=>x.Catalog)
                .AsNoTracking();

            if(request.Filter != null)
            {
                query = query.Where(request.Filter);
            }           

            var count = query.Count();
            var pages = count / request.PageSize + (count % request.PageSize == 0 ? 1 : 0);

            if (request.Sort != null)
            {
                query = query.OrderBy(request.Sort);
            }
            
            var items = await query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();            
            
            return new GetProductListResponse
            {
                Items = items,
                Page = request.Page,
                TotalPages = pages
            };
        }

        public override async Task<Product> UpdateProduct(UpdateProductRequest request)
        {
            Product product = request.Product;

            if(request.Data != null)
            {
                product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (product == null)
                    throw new Exception("Product Not Found");

                product.Merge(request.Data);             
            }
            else
            {                
                product.Id = request.Id;
                var entry = _context.Products.Attach(product);
                entry.State = EntityState.Modified;
            }

            product.UpdateDate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception($"Product {request.Id} not found!");
            }

            return product;
        }
    }
}
