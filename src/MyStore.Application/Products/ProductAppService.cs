using Microsoft.AspNetCore.Authorization;
using MyStore.Permissions;
using MyStore.Products;
using MyStore.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyStore.Products
{
    [Authorize(MyStorePermissions.Products.Default)]
    public class ProductAppService : CrudAppService<
                                        Product,
                                        ProductDto,
                                        Guid,
                                        PagedAndSortedResultRequestDto,
                                        CreateUpdateProductDto>,
                                    IProductAppService
    {
        public ProductAppService(IRepository<Product, Guid> repository) : base(repository)
        {
            GetPolicyName = MyStorePermissions.Products.Default;
            GetListPolicyName = MyStorePermissions.Products.Default;
            CreatePolicyName = MyStorePermissions.Products.Create;
            UpdatePolicyName = MyStorePermissions.Products.Edit;
            DeletePolicyName = MyStorePermissions.Products.Delete;
        }

    }
}
