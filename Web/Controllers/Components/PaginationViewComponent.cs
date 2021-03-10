using System.Threading.Tasks;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Controllers.Components
{
    public class PaginationViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResult<ProductViewModel> result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}