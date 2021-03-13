using System.Collections.Generic;
using System.Threading.Tasks;
using eShop_Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eShop_Mvc.Controllers.Components
{
    public class HeaderCartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var session = HttpContext.Session.GetString("CartSession");
            var cart = new List<CartViewModel>();
            if (session != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartViewModel>>(session);
            }

            return View(cart);
        }
    }
}