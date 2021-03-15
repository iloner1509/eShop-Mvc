using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace eShop_Mvc.Controllers.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public CategoryMenuViewComponent(IProductCategoryService productCategoryService, IMapper mapper, IMemoryCache memoryCache)
        {
            _productCategoryService = productCategoryService;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var categoryList = await _productCategoryService.GetAllAsync();
            var categories = await _memoryCache.GetOrCreateAsync(CacheKeys.ProductCategories, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(8);
                return _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(
                    await _productCategoryService.GetAllAsync());
            });
            return View((categories));
        }
    }
}