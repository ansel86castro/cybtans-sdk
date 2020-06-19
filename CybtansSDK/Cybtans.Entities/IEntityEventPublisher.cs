using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cybtans.Entities
{

    public interface IEntityEventPublisher
    {
        Task Publish(EntityEvent entityEvent);

        Task PublishAll(IEnumerable<EntityEvent> entityEvent);
    }  
    
}
