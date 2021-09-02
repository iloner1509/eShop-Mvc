using eShop_Mvc.Models.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(AuthenticationSchemes = AuthenticationSchemes.AdminAuthenticationScheme)]
    [Authorize]
    public class BaseController : Controller
    {
    }
}