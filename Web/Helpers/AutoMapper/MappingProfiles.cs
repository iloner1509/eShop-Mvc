using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Models;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.Models.System;

namespace eShop_Mvc.Helpers.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Function, FunctionViewModel>();
        }
    }
}