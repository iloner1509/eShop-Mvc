using System.Threading.Tasks;
using eShop_Mvc.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Controllers.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryService _productCategoryService;

        public CategoryMenuViewComponent(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _productCategoryService.GetAllAsync());
        }
    }
}