using System.Globalization;
using AutoMapper;
using eShop_Mvc.Areas.Admin.Models;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Models.AccountViewModels;
using eShop_Mvc.Models.Common;
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
            CreateMap<AppUser, AppUserViewModel>()
                .AfterMap((src, dest) => dest.BirthDay = src.BirthDay?.ToString("dd/MM/yyyy"));
            CreateMap<AppRole, AppRoleViewModel>().ReverseMap();
            CreateMap<Permission, PermissionViewModel>().ReverseMap();
            CreateMap<Announcement, AnnouncementViewModel>().MaxDepth(2).ReverseMap();
            CreateMap<AnnouncementUser, AnnouncementUserViewModel>().ReverseMap();
            CreateMap<Bill, ProductViewModel>().MaxDepth(2).ReverseMap();
            CreateMap<BillDetail, BillDetailViewModel>().MaxDepth(2).ReverseMap();
            CreateMap<Tag, TagViewModel>().ReverseMap();
        }
    }
}