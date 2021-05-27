using Microsoft.EntityFrameworkCore;

#nullable enable

namespace Cybtans.Entities.EntityFrameworkCore
{
    public interface IDbContextUnitOfWork : IUnitOfWork
    {
        DbContext Context { get; }

        DbContext GetContext(ReadConsistency consistency);
    }
}
