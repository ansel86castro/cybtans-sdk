using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Catalog.Clients;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class CreateProductModel : PageModel
    {      
        private readonly ICatalogService _catalogService;

        public CreateProductModel(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [BindProperty]
        public Product Product { get; set; }

        public void OnGet()
        {
            Product = new Product();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Product.BrandId = 1;
            Product.CatalogId = 1;

            Product = await _catalogService.CreateProduct(Product);

            return RedirectToPage("./Index");
        }
    }
}