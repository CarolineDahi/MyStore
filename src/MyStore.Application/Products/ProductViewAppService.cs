using Microsoft.AspNetCore.Authorization;
using Microsoft.Win32;
using MyStore.Customers;
using MyStore.Products.Dtos;
using MyStore.Settings;
using MyStore.Users;
using Polly;
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
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace MyStore.Products
{
    public class ProductViewAppService : CrudAppService<
                                            ProductView,
                                            ProductViewDto,
                                            Guid,
                                            PagedAndSortedResultRequestDto,
                                            CreateUpdateProductViewDto>,
                                        IProductViewAppService
    {
        private readonly IRepository<ProductView, Guid> repository;
        private readonly ISettingProvider setting;
        private readonly ISettingManager settingManager;
        private readonly IRepository<Customer, Guid> customerRepository;
        private readonly IIdentityUserRepository identityUserRepository;

        public ProductViewAppService(IRepository<ProductView, Guid> repository, 
                                     ISettingProvider setting,
                                     ISettingManager settingManager,
                                     IRepository<Customer, Guid> customerRepository,
                                     IIdentityUserRepository identityUserRepository) : base(repository)
        {
            this.repository = repository;
            this.setting = setting;
            this.settingManager = settingManager;
            this.customerRepository = customerRepository;
            this.identityUserRepository = identityUserRepository;
        }

        public async override Task<PagedResultDto<ProductViewDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var query = await repository.GetQueryableAsync();
            var reviews = query.Where(pv => pv.IsApproved == true).ToList();

            var reviewsDtos = ObjectMapper.Map<List<ProductView>, List<ProductViewDto>>(reviews);

            return new PagedResultDto<ProductViewDto>(reviewsDtos.Count, reviewsDtos);
        }

        public async override Task<ProductViewDto> CreateAsync(CreateUpdateProductViewDto input)
        {
            var customer = await customerRepository.GetAsync(input.CustomerId);

            var user = await identityUserRepository.GetAsync(customer.UserId);
            var type = user.GetProperty<byte>("UserType");
            if (type != (byte)UserType.Customer)
            {
                throw new UnauthorizedAccessException();
            }

            var AutoApprove = await setting.IsTrueAsync("MyStoreAutoApproveReviewer");

            var reViewer = new ProductView
            {
                Comment = input.Comment,
                Rate = input.Rate,
                ProductId = input.ProductId,
                CustomerId = input.CustomerId,
                IsApproved = AutoApprove,
                RateDate = input.RateDate,
            };

            await repository.InsertAsync(reViewer);

            return ObjectMapper.Map<ProductView, ProductViewDto>(reViewer);
        }

        public async Task ApproveReviews(List<Guid> viewIds, bool IsApprove)
        {
            var query = await repository.GetQueryableAsync();
            var views = query.Where(v => viewIds.Contains(v.Id)).ToList();

            views.ForEach(v => v.IsApproved = IsApprove);
            await repository.UpdateManyAsync(views);
        }

        public async Task ChangeAutoApproved(bool IsAutoApproved)
        {
            await settingManager.SetGlobalAsync("MyStoreAutoApproveReviewer", IsAutoApproved ? "true" : "null");
        }
    }
}
