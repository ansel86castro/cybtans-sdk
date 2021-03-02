using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cybtans.Entities
{
    public interface IIncludableQueryable<out TEntity, out TProperty> : IQueryable<TEntity>
        where TEntity:class 

    {
    }    
}
