using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Clients;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class EditModel : PageModel
    {
        private readonly ICatalogService _catalogService;

        public EditModel(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _catalogService.GetProduct(id);

            if (Product == null)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Product = await _catalogService.UpdateProduct(new UpdateProductRequest
            {
                Id = Product.Id,
                Data = new Dictionary<string, object>
                {
                    ["Name"] = Product.Name,
                    ["Description"] = Product.Description,
                    ["Price"] = Product.Price
                }
            });

            return RedirectToPage("./Index");
        }
    }
}