using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyStore.Products.Data_Access
{
    public interface IProductViewRepository : IRepository<ProductView, Guid>
    {
        Task<IQueryable<ProductView>> DetailsWithInclude();
    }
}
