using MyStore.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyStore.Products
{
    public interface IProductAppService : ICrudAppService<
                                                ProductDto,
                                                Guid,
                                                PagedAndSortedResultRequestDto, 
                                                CreateUpdateProductDto> 
    {
    }
}
