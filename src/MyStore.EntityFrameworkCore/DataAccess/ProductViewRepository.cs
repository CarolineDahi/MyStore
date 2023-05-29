using Microsoft.EntityFrameworkCore;
using MyStore.EntityFrameworkCore;
using MyStore.Products;
using MyStore.Products.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyStore.DataAccess
{
    public class ProductViewRepository : EfCoreRepository<MyStoreDbContext, ProductView, Guid>, IProductViewRepository
    {
        public ProductViewRepository(IDbContextProvider<MyStoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<IQueryable<ProductView>> DetailsWithInclude()
        {
            var query = await GetQueryableAsync();
            var reViews = query.Include(r => r.Product)
                               .Include(r => r.Customer)
                               ;
            return reViews;
        }

    }
}
