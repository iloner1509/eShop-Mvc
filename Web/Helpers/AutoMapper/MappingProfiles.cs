using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Models;
using eShop_Mvc.Models.AccountViewModels;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.Models.System;

namespace eShop_Mvc.Helpers.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Function, FunctionViewModel>().ReverseMap();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>().ReverseMap();
            CreateMap<Permission, PermissionViewModel>().ReverseMap();
            CreateMap<Announcement, AnnouncementViewModel>().MaxDepth(2).ReverseMap();
            CreateMap<AnnouncementUser, AnnouncementUserViewModel>().ReverseMap();
        }
    }
}