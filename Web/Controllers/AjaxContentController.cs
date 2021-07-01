using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult HeaderCart()
        {
            return ViewComponent("HeaderCart");
        }
    }
}