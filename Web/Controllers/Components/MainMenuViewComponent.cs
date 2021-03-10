using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Controllers.Components
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;

        public MainMenuViewComponent(IProductCategoryService productCategoryService, IMapper mapper)
        {
            _productCategoryService = productCategoryService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryList = await _productCategoryService.GetAllAsync();
            return View(_mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(categoryList));
        }
    }
}