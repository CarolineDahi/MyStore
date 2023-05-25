using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Products.Dtos
{
    public class CreateUpdateProductViewDto
    {
        public double Rate { get; set; }
        public string Comment { get; set; }
        public DateTime RateDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
    }
}
