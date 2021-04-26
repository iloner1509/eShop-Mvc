using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Remove("LoginSession");
            return Redirect("/Admin/Login/Index");
        }
    }
}