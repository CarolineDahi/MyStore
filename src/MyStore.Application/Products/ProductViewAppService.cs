using Microsoft.AspNetCore.Authorization;
using Microsoft.Win32;
using MyStore.Customers;
using MyStore.Permissions;
using MyStore.Products.Dtos;
using MyStore.Settings;
using MyStore.Users;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace MyStore.Products
{
    [Authorize(MyStorePermissions.ProductViews.Default)]
    public class ProductViewAppService : CrudAppService<
                                            ProductView,
                                            ProductViewDto,
                                            Guid,
                                            PagedAndSortedResultRequestDto,
                                            CreateUpdateProductViewDto>,
                                        IProductViewAppService
    {
        private readonly ISettingProvider setting;
        private readonly ISettingManager settingManager;
        private readonly IRepository<Customer, Guid> customerRepository;
        private readonly IRepository<Product, Guid> productRepository;
        private readonly IIdentityUserRepository identityUserRepository;

        public ProductViewAppService(IRepository<ProductView, Guid> repository, 
                                     ISettingProvider setting,
                                     ISettingManager settingManager,
                                     IRepository<Customer, Guid> customerRepository,
                                     IRepository<Product, Guid> productRepository,
                                     IIdentityUserRepository identityUserRepository) : base(repository)
        {
            this.setting = setting;
            this.settingManager = settingManager;
            this.customerRepository = customerRepository;
            this.productRepository = productRepository;
            this.identityUserRepository = identityUserRepository;


            GetPolicyName = MyStorePermissions.ProductViews.Default;
            GetListPolicyName = MyStorePermissions.ProductViews.Default;
            CreatePolicyName = MyStorePermissions.ProductViews.Create;
            UpdatePolicyName = MyStorePermissions.ProductViews.Edit;
            DeletePolicyName = MyStorePermissions.ProductViews.Delete;
        }

        public async override Task<ProductViewDto> GetAsync(Guid id)
        {
            var review = await Repository.GetAsync(id);
            var reviewDto = ObjectMapper.Map<ProductView, ProductViewDto>(review);

            var customer = await customerRepository.GetAsync(review.CustomerId);
            reviewDto.CustomerName = customer.FirstName + " " + customer.LastName;

            var product = await productRepository.GetAsync(review.ProductId);
            reviewDto.ProductName = product.Name;

            return reviewDto;
        }


        public async override Task<PagedResultDto<ProductViewDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
                input.Sorting = nameof(ProductView.RateDate);

            var user = await identityUserRepository.GetAsync(CurrentUser.Id.Value);
            var type = user.GetProperty<byte>("UserType");

            var query = await Repository.WithDetailsAsync();
       
            if (type is (byte)UserType.Customer)
                query = query.Where(r => r.IsApproved == true);

            var reviews = await AsyncExecuter.ToListAsync( 
                query.OrderBy(input.Sorting)
                     .Skip(input.SkipCount)
                     .Take(input.MaxResultCount)
                     );

            var reviewsDtos = ObjectMapper.Map<List<ProductView>, List<ProductViewDto>>(reviews);

            var customerDic = await GetCustomerDictionaryAsync(reviews);
            reviewsDtos.ForEach(r => r.CustomerName =
            customerDic[r.CustomerId].FirstName + " " + customerDic[r.CustomerId].LastName);

            var productDic = await GetProductDictionaryAsync(reviews);
            reviewsDtos.ForEach(r => r.ProductName =
            productDic[r.ProductId].Name);

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
            var review = ObjectMapper.Map<CreateUpdateProductViewDto, ProductView>(input);
            review.IsApproved = AutoApprove;

            await Repository.InsertAsync(review);

            return ObjectMapper.Map<ProductView, ProductViewDto>(review);
        }

        public async Task ApproveReviews(List<Guid> viewIds, bool IsApprove)
        {
            var query = await Repository.GetQueryableAsync();
            var views = query.Where(v => viewIds.Contains(v.Id)).ToList();

            views.ForEach(v => v.IsApproved = IsApprove);
            await Repository.UpdateManyAsync(views);
        }

        public async Task ChangeAutoApproved(bool IsAutoApproved)
        {
            await settingManager.SetGlobalAsync("MyStoreAutoApproveReviewer", IsAutoApproved.ToString());
        }

        private async Task<Dictionary<Guid, Customer>> GetCustomerDictionaryAsync(List<ProductView> reviews)
        {
            var customerIds = reviews.Select(r => r.CustomerId).Distinct();

            var query = await customerRepository.GetQueryableAsync();

            var customers = await AsyncExecuter.ToListAsync(
                query.Where(c => customerIds.Contains(c.Id)));

            return customers.ToDictionary(c => c.Id, c => c);
        }

        private async Task<Dictionary<Guid, Product>> GetProductDictionaryAsync(List<ProductView> reviews)
        {
            var productIds = reviews.Select(r => r.ProductId).Distinct();

            var query = await productRepository.GetQueryableAsync();

            var products = await AsyncExecuter.ToListAsync(
                query.Where(c => productIds.Contains(c.Id)));

            return products.ToDictionary(c => c.Id, c => c);
        }
    }
}
