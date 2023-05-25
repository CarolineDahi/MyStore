using MyStore.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyStore.Products
{
    public interface IProductViewAppService : ICrudAppService<
                                                    ProductViewDto,
                                                    Guid,
                                                    PagedAndSortedResultRequestDto,
                                                    CreateUpdateProductViewDto>
    {
        Task ApproveReviews(List<Guid> viewIds,bool IsApprove);
        Task ChangeAutoApproved(bool IsAutoApproved);

    }
}
