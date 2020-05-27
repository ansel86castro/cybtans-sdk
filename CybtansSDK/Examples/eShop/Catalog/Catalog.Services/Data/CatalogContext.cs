using Catalog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Catalog.Services.Data
{   
    public class CatalogContext:DbContext
    {       
        public CatalogContext(DbContextOptions<CatalogContext> options)
          : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {          
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                // optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=admin;database=alquileres");
            }
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Models.Catalog> Catalogs { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public static void Initialize(CatalogContext context)
        {            
            var brands = Enumerable.Range(1, 5).Select(i => new Brand {  Name = $"Brand {i}" }).ToList();
            var catalogs = Enumerable.Range(1, 3).Select(i => new Models.Catalog { Name = $"Catalog {i}" });

            context.Brands.AddRange(brands);
            context.Catalogs.AddRange(catalogs);

            context.SaveChanges();
         
            var rand = new Random();
            foreach (var catalog in catalogs)
            {
                context.Products.AddRange(Enumerable.Range(1, 10)
                    .Select(i => new Product
                    {
                        Brand = brands[i % 5],
                        Catalog = catalog,
                        AvalaibleStock = rand.Next(1000),
                        CreateDate = DateTime.Now,
                        Name = $"Product {catalog.Id}-{i}",
                        Price = i % 2 == 0 ? i : float.Parse($"{rand.Next(100, 500)}.{rand.Next(0, 100)}"),
                        RestockThreshold = 5,
                        Description = $"Description for Product {catalog.Id}{i}",
                        Comments = new List<Comment>()
                    }));
               
            }

            context.SaveChanges();
        }
    }
}
