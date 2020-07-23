using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public interface IQueryFilter<T>
        where T:class
    {
        IQueryable<T> Query(DbContext context, DbSet<T> dbSet);
    }

    public interface IQueryFilter        
    {
        IQueryable<T> Query<T>(DbContext context, DbSet<T> dbSet) where T : class;

        
    }


}
