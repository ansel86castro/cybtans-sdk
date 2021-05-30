using Cybtans.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public class EfEntityRepository<T, TKey> : EfRepository<T, TKey>, IEntityRepository<T, TKey>
      where T : class, IEntity<TKey>
      where TKey : IEquatable<TKey>
    {
        public EfEntityRepository(IDbContextUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async ValueTask<T> Get(TKey key, bool includeDetails = true, ReadConsistency consistency = ReadConsistency.Default)
        {
            if (includeDetails)
            {
                IQueryable<T> query = WithDetailsSingle(consistency);
                return await query.FirstOrDefaultAsync(x => x.Id.Equals(key));
            }

            return await base.Get(key, includeDetails, consistency);
        }
     
    }


    public interface IEntityKey<T>
    {
       Expression<Func<T, bool>> GetFilter();

        object[] GetValues();
    }
}
