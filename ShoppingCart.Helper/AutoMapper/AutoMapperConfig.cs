using AutoMapper;
using ShoppingCart.Model;
using ShoppingCart.Provider.EntityModel;

namespace ShoppingCart.Helper.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MasterProfile());
            });
        }
    }

    public class MasterProfile : Profile
    {
        public MasterProfile()
        {
            CreateMap<UserModel, UserDetail>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<CategoryModel, Category>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<SubCategoryModel, SubCategory>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<ProductModel, Product>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<AddressModel, AddressDetail>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<OrderModel, OrderDetail>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
