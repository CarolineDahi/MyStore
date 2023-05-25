using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MyStore.Products.Dtos
{
    public class ProductViewDto : AuditedEntityDto<Guid>
    {
        public double Rate { get; set; }
        public string Comment { get; set; }
        public DateTime RateDate { get; set; }
        public bool? IsApproved { get; set; }

        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
