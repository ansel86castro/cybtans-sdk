using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Test.Domain.EF
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<AdventureContext>
    {
        public AdventureContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdventureContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=AdventureWorks2014;Trusted_Connection=True;");

            return new AdventureContext(optionsBuilder.Options);
        }
    }
}
