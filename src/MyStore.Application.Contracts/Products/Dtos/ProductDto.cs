using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MyStore.Products.Dtos
{
    public class ProductDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double RatingSum { get; set; }
        public double TotalVotes { get; set; }
        public double AvrageRate { get; set; }
    }
}
