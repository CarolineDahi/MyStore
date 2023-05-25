using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyStore.Products
{
    public class Product : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double RatingSum { get; set; }
        public double TotalVotes { get; set; }

        public ICollection<ProductView> ProductViews { get; set; }
    }
}
