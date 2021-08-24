using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Services.Command.UserCommand;
using eShop_Mvc.Core.Services.Query.UserQuery;
using eShop_Mvc.Models.AccountViewModels;
using eShop_Mvc.SharedKernel.Enums;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eShop_Mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger, IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            LoginViewModel model = new LoginViewModel()
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new LoginQuery()
                {
                    Username = model.Username,
                    Password = model.Password,
                    RememberMe = model.RememberMe
                });
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User {model.Username} logged in");
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    return LocalRedirect(model.ReturnUrl);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User locked out");
                    return RedirectToAction(nameof(Login));
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View(model);
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            string redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string serviceError = null)
        {
            returnUrl ??= Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (serviceError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {serviceError}");
                return View(nameof(Login), loginViewModel);
            }
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo =
                {info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value};
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {userInfo[0]} has logged in via Google");
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                return LocalRedirect(returnUrl);
            }
            else
            {
                AppUser user = new AppUser()
                {
                    UserName = userInfo[1],
                    Email = userInfo[1],
                    FullName = userInfo[0],
                    Status = Status.Active,
                    Avatar = string.Empty
                };
                var createUserResult = await _userManager.CreateAsync(user);
                if (createUserResult.Succeeded)
                {
                    var identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        _logger.LogInformation($"User {user.UserName} was successfully created !");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                        return LocalRedirect(returnUrl);
                    }
                }

                return RedirectToAction(nameof(Login));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User signed out");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [ValidateRecaptcha]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new AppUser()
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                BirthDay = model.DateOfBirth,
                Status = Status.Active,
                Avatar = string.Empty
            };
            var result = await _mediator.Send(new CreateUserCommand()
            {
                AppUser = user,
                Password = model.Password
            });
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.UserName} was successfully created !");
                await _signInManager.SignInAsync(user, false);
                return RedirectToLocal(returnUrl);
            }
            AddError(result);
            return View(model);
        }

        #region private method

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private void AddError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion private method
    }
}