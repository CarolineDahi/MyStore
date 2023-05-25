using MyStore.Products;
using MyStore.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace MyStore.Customers
{
    public class Customer :  FullAuditedAggregateRoot<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Image { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PreferredLanguage { get; set; }
        public string Number { get; set; }

        public Guid UserId { get; set; }
        public IdentityUser User { get; set; }

        public ICollection<ProductView> ProductViews { get; set; }
    }
}
