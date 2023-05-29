using Microsoft.AspNetCore.Authorization;
using Microsoft.Win32;
using MyStore.Customers;
using MyStore.Permissions;
using MyStore.Products.Data_Access;
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
using Volo.Abp.Validation;

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
        private readonly IProductViewRepository repository;
        private readonly ISettingProvider setting;
        private readonly ISettingManager settingManager;
        private readonly IRepository<Customer, Guid> customerRepository;
        private readonly IRepository<Product, Guid> productRepository;
        private readonly IIdentityUserRepository identityUserRepository;

        public ProductViewAppService(IProductViewRepository repository, 
                                     ISettingProvider setting,
                                     ISettingManager settingManager,
                                     IRepository<Customer, Guid> customerRepository,
                                     IRepository<Product, Guid> productRepository,
                                     IIdentityUserRepository identityUserRepository) : base(repository)
        {
            this.repository = repository;
            this.setting = setting;
            this.settingManager = settingManager;
            this.customerRepository = customerRepository;
            this.productRepository = productRepository;
            this.identityUserRepository = identityUserRepository;

        }

        public async override Task<ProductViewDto> GetAsync(Guid id)
        {

            var review = (await repository.DetailsWithInclude()).SingleOrDefault(r => r.Id == id);
            
            return ObjectMapper.Map<ProductView, ProductViewDto>(review);
        }


        public async override Task<PagedResultDto<ProductViewDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
                input.Sorting = nameof(ProductView.RateDate);

            var user = await identityUserRepository.GetAsync(CurrentUser.Id.Value);
            var type = user.GetProperty<byte>("UserType");

            var query = await repository.DetailsWithInclude();
       
            if (type is (byte)UserType.Customer)
                query = query.Where(r => r.IsApproved == true);

            var reviews = await AsyncExecuter.ToListAsync( 
                query.OrderBy(input.Sorting)
                     .Skip(input.SkipCount)
                     .Take(input.MaxResultCount)
                     );

            var reviewsDtos = ObjectMapper.Map<List<ProductView>, List<ProductViewDto>>(reviews);

            return new PagedResultDto<ProductViewDto>(reviewsDtos.Count, reviewsDtos);
        }

        public async override Task<ProductViewDto> CreateAsync(CreateUpdateProductViewDto input)
        {
            if(!CurrentUser.Id.HasValue)
            {
                throw new InvalidOperationException("Guest can not rate.");
            }

            var user = await identityUserRepository.GetAsync(CurrentUser.Id.Value);
            var customerId = user.GetProperty<Guid?>("TypeId");
            if(customerId != input.CustomerId)
            {
                throw new InvalidOperationException("CustomerId not equal Logged user id");
            }

            var type = user.GetProperty<byte>("UserType");
            if (type != (byte)UserType.Customer)
            {
                throw new UnauthorizedAccessException();
            }

            var reviewOld = await Repository.SingleOrDefaultAsync(r => r.ProductId == input.ProductId 
                                                                    && r.CustomerId == input.CustomerId);
            if(reviewOld is not null)
            {
                throw new InvalidOperationException("You can't add another review");
            }


            var AutoApprove = await setting.IsTrueAsync("MyStoreAutoApproveReviewer");
            var review = ObjectMapper.Map<CreateUpdateProductViewDto, ProductView>(input);
            review.IsApproved = AutoApprove ? AutoApprove : null;
            review.RateDate = DateTime.Now;

            await Repository.InsertAsync(review, true);

            if (AutoApprove)
            {
                var product = await productRepository.GetAsync(input.ProductId);
                product.RatingSum += input.Rate;
                product.TotalVotes++;

                await productRepository.UpdateAsync(product);
            }

            review = (await repository.DetailsWithInclude()).SingleOrDefault(r => r.Id == review.Id);

            return ObjectMapper.Map<ProductView, ProductViewDto>(review);
        }

        public async override Task<ProductViewDto> UpdateAsync(Guid id, CreateUpdateProductViewDto input)
        {
            var user = await identityUserRepository.GetAsync(CurrentUser.Id.Value);
            var customerId = user.GetProperty<Guid?>("TypeId");
            if (customerId != input.CustomerId)
            {
                throw new InvalidOperationException("CustomerId not equal Logged user id");
            }

            var type = user.GetProperty<byte>("UserType");
            if(type != (byte)UserType.Customer)
            { 
                throw new UnauthorizedAccessException(); 
            }

            var review = await Repository.GetAsync(id);

            if (review.IsApproved is true)
            {
                var product = await productRepository.GetAsync(input.ProductId);
                product.RatingSum += (-review.Rate + input.Rate);
                await productRepository.UpdateAsync(product);
            }

            review.Rate = input.Rate;
            review.Comment = input.Comment;
            await Repository.UpdateAsync(review, true);
            
            review = (await repository.DetailsWithInclude()).SingleOrDefault(r => r.Id == review.Id);

            return ObjectMapper.Map<ProductView, ProductViewDto>(review);
        }

        public async override Task DeleteAsync(Guid id)
        {
            var review = await Repository.GetAsync(id);

            if (review.IsApproved is true)
            {
                var product = await productRepository.GetAsync(review.ProductId);
                product.RatingSum -= review.Rate;
                product.TotalVotes--;
                await productRepository.UpdateAsync(product);
            }
            await Repository.DeleteAsync(review);
        }

        public async Task ApproveReviews(List<Guid> viewIds, bool IsApprove)
        {
            var query = await repository.DetailsWithInclude();
            var reviews = query.Where(v => viewIds.Contains(v.Id)).ToList();

            reviews.ForEach(v => v.IsApproved = IsApprove);

            if(IsApprove)
            {
                foreach (var review in reviews)
                {
                    review.Product.RatingSum += review.Rate;
                    review.Product.TotalVotes++;
                    await productRepository.UpdateAsync(review.Product);
                }
            }

            await Repository.UpdateManyAsync(reviews);
        }

        public async Task ChangeAutoApproved(bool IsAutoApproved)
        {
            await settingManager.SetGlobalAsync("MyStoreAutoApproveReviewer", IsAutoApproved.ToString());
        }

    }
}
