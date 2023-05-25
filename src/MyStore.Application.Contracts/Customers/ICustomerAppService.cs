using MyStore.Customers.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyStore.Customers
{
    public interface ICustomerAppService : ICrudAppService<
                                                CustomerDto,
                                                Guid,
                                                PagedAndSortedResultRequestDto,
                                                CreateUpdateCustomerDto>
    {
    }
}
