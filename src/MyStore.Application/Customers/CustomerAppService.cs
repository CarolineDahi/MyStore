using Microsoft.AspNetCore.Authorization;
using MyStore.Customers.Dtos;
using MyStore.Permissions;
using MyStore.Products;
using MyStore.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace MyStore.Customers
{
    //[Authorize(MyStorePermissions.Customers.Default)]
    public class CustomerAppService : CrudAppService<
                                            Customer,
                                            CustomerDto,
                                            Guid,
                                            PagedAndSortedResultRequestDto,
                                            CreateUpdateCustomerDto>,
                                      ICustomerAppService

    {
        private readonly IdentityUserManager userAppService;

        public CustomerAppService(IRepository<Customer, Guid> repository, 
                                  IdentityUserManager userAppService) : base(repository)
        {
            this.userAppService = userAppService;

            //GetPolicyName = MyStorePermissions.Customers.Default;
            //GetListPolicyName = MyStorePermissions.Customers.Default;
            //UpdatePolicyName = MyStorePermissions.Customers.Edit;
            //DeletePolicyName = MyStorePermissions.Customers.Delete;
        }

        [AllowAnonymous]
        public async override Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
        {
            var user = new IdentityUser(new Guid(), input.UserName, input.Email);
            await userAppService.CreateAsync(user, input.Password);

            var customer = ObjectMapper.Map<CreateUpdateCustomerDto, Customer>(input);
            customer.UserId = user.Id;

            await Repository.InsertAsync(customer);

            user.SetProperty("TypeId", customer.Id);
            user.SetProperty("UserType", UserType.Customer);
            await userAppService.UpdateAsync(user);

            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        
    }
}
