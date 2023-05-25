using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MyStore.Customers.Dtos
{
    public class CustomerDto : AuditedEntityDto<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PreferredLanguage { get; set; }
        public string Number { get; set; }
    }
}
