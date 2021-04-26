using System;
using System.Linq;
using System.Security.Claims;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Dtos;
using eShop_Mvc.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using eShop_Mvc.Extensions;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(SignInManager<AppUser> signInManager, ILogger<LoginController> logger, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged id");
                    var currentUser = await _userManager.FindByNameAsync(model.Username);
                    var session = new AppUserViewModel()
                    {
                        Id = currentUser.Id,
                        FullName = currentUser.FullName,
                        Avatar = currentUser.Avatar,
                        UserName = currentUser.UserName,
                    };
                    HttpContext.Session.Set("LoginSession", session);

                    return new ObjectResult(new GenericResult(true));
                }

                if (result.IsLockedOut)
                {
                    _logger.LogInformation("User account has been locked out");
                    return new ObjectResult(new GenericResult(false, "Tài khoản bị khóa"));
                }
                else
                {
                    return new ObjectResult(new GenericResult(false, "Sai tên tài khoản hoặc mật khẩu"));
                }
            }

            return new ObjectResult(new GenericResult(false, model));
        }
    }
}