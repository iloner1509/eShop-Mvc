﻿using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Services.Query.UserQuery;
using eShop_Mvc.Dtos;
using eShop_Mvc.Extensions;
using eShop_Mvc.Models.AccountViewModels;
using MediatR;
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
        private readonly ILogger<LoginController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;

        public LoginController(ILogger<LoginController> logger, UserManager<AppUser> userManager, IMediator mediator)
        {
            _logger = logger;
            _userManager = userManager;
            _mediator = mediator;
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
                var result = await _mediator.Send(new LoginQuery()
                {
                    Username = model.Username,
                    Password = model.Password,
                    RememberMe = model.RememberMe
                });
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

                return new ObjectResult(new GenericResult(false, "Sai tên tài khoản hoặc mật khẩu"));
            }

            return new ObjectResult(new GenericResult(false, model));
        }
    }
}