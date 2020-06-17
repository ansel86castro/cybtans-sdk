using System;

namespace Cybtans.Entities
{
    public interface IAuditableEntity :IEntity
    {
        DateTime CreateDate { get; set; }     
        DateTime? UpdateDate { get; set; }
        string Creator { get; set; }
    }
}
