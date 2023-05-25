using MyStore.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace MyStore.Products
{
    public class ProductAppService_Tests : MyStoreApplicationTestBase
    {
        private IProductAppService _productService;

        public ProductAppService_Tests()
        {
            _productService = GetRequiredService<IProductAppService>();
        }

        [Fact]
        public async Task Should_Get_List_Of_Products()
        {
            //Act
            var result = await _productService.GetListAsync(
                new PagedAndSortedResultRequestDto()
            );

        }

        [Fact]
        public async Task Should_Create_A_Valid_Product()
        {
            //Act
            var result = await _productService.CreateAsync(
                new CreateUpdateProductDto
                {
                    Name = "Test 1",
                    Description = "Description",
                    Price = 10,
                }
            );

        }
    }
}
