using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Services.Query.CategoryQuery;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop_Mvc.Controllers.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public CategoryMenuViewComponent(IMediator mediator, IMapper mapper, IMemoryCache memoryCache)
        {
            _mediator = mediator;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var categoryList = await _productCategoryService.GetAllAsync();
            var categories = await _memoryCache.GetOrCreateAsync(CacheKeys.ProductCategories, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(8);
                return _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(await _mediator.Send(new GetAllCategoryQuery()));
            });
            return View(categories);
        }
    }
}