using AutoMapper;
using MyStore.Customers;
using MyStore.Customers.Dtos;
using MyStore.Products;
using MyStore.Products.Dtos;

namespace MyStore;

public class MyStoreApplicationAutoMapperProfile : Profile
{
    public MyStoreApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();
        CreateMap<ProductView, ProductViewDto>()
            .ForMember(dto => dto.CustomerName, ops => ops.MapFrom(src => src.Customer.FirstName));
        CreateMap<CreateUpdateProductViewDto, ProductView>();
    }
}
