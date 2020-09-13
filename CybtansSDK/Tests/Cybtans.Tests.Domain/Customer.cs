using Cybtans.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Test.Domain
{
    [Description("Customer Entity")]
    [GenerateMessage(Service = ServiceType.Interface)]
    public class Customer: DomainTenantEntity<Guid>
    {
        [Description("Customer's Name")]
        [Required]
        [EventData]
        public string Name { get; set; }
       
        [Description("Customer's FirstLastName")]
        [EventData]        
        public string FirstLastName { get; set; }

        [Description("Customer's SecondLastName")]
        [EventData]
        [Obsolete]
        public string SecondLastName { get; set; }

        [Description("Customer's Profile Id, can be null")]
        [EventData]
        public Guid? CustomerProfileId { get; set; }

        [EventData]
        public virtual CustomerProfile CustomerProfile { get; set; }
    }
}
