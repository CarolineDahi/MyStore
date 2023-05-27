using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Customers.Dtos
{
    public class CreateUpdateCustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PreferredLanguage { get; set; }
        public string Number { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
