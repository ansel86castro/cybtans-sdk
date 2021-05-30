#nullable enable

using Microsoft.EntityFrameworkCore;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public class ReadScaleOutUnitOfWork : EfUnitOfWork
    {
        private readonly DbContext _readOnlyContext;

        public ReadScaleOutUnitOfWork(DbContext context, DbContext readOnlyContext, IEntityEventPublisher? eventPublisher) : base(context, eventPublisher)
        {
            _readOnlyContext = readOnlyContext;
        }

        public override DbContext GetContext(ReadConsistency consistency)
        {
            switch (consistency)
            {
                case ReadConsistency.Strong: return Context;
                case ReadConsistency.Weak: return _readOnlyContext;
                default: return Context;
            }
        }
    }
}
