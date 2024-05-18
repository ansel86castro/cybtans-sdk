using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cybtans.AspNetCore.Migrations
{
    public interface IDbContextSeed<TContext>
        where TContext :DbContext
    {
        Task Seed(TContext dbContext);
    }
}
