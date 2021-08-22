using System;
using System.Linq.Expressions;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public interface IEntityKey<T>
    {
       Expression<Func<T, bool>> GetFilter();

        object[] GetValues();
    }
}
