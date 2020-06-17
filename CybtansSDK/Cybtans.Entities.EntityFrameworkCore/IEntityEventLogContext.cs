using Cybtans.Entities.EventLog;
using Microsoft.EntityFrameworkCore;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public interface IEntityEventLogContext
    {
        DbSet<EntityEventLog> EntityEventLogs { get; }
    }
}
