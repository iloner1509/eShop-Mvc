using eShop_Mvc.Core.Entities;
using eShop_Mvc.Dtos;
using eShop_Mvc.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(SignInManager<AppUser> signInManager, ILogger<LoginController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
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