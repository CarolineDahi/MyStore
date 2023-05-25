using MyStore.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyStore.Products
{
    public class ProductView : FullAuditedAggregateRoot<Guid>
    {

        public double Rate { get; set; }
        public string Comment { get; set; }
        public DateTime RateDate { get; set; }
        public bool? IsApproved { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
