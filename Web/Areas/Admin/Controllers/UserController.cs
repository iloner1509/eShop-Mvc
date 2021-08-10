using AutoMapper;
using eShop_Mvc.Authorization;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Extensions;
using eShop_Mvc.Models.AccountViewModels;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eShop_Mvc.Areas.Admin.Models;
using eShop_Mvc.Models.System;
using eShop_Mvc.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly IHubContext<AnnoucementHub, IAnnouncementHub> _announcementHubContext;

        public UserController(UserManager<AppUser> userManager, IMapper mapper,
                              IAuthorizationService authorizationService, SignInManager<AppUser> signInManager,
                              ILogger<UserController> logger, IHubContext<AnnoucementHub, IAnnouncementHub> announcementHubContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _signInManager = signInManager;
            _logger = logger;
            _announcementHubContext = announcementHubContext;
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _userManager.FindByIdAsync(userId);
            return View(_mapper.Map<AppUser, AppUserViewModel>(model));
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppUserViewModel appUser)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(m => m.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (appUser.Id == null)
            {
                var user = new AppUser()
                {
                    UserName = appUser.UserName,
                    Avatar = appUser.Avatar,
                    Email = appUser.Email,
                    FullName = appUser.FullName,
                    DateCreated = DateTime.Now,
                    PhoneNumber = appUser.PhoneNumber,
                    Status = appUser.Status
                };
                var result = await _userManager.CreateAsync(user, appUser.Password);
                if (result.Succeeded)
                {
                    if (appUser.Roles != null)
                    {
                        var createdUser = await _userManager.FindByNameAsync(user.UserName);
                        if (createdUser != null)
                        {
                            await _userManager.AddToRolesAsync(createdUser, appUser.Roles);
                        }
                    }
                    _logger.LogInformation($"User {user.UserName} has been created successfully!");

                    // send announcement to all user
                    var announcement = new AnnouncementViewModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = $"New user {user.UserName} is added into user list.",
                        CreatedBy = User.FindFirst(ClaimTypes.Name).Value,
                        TimeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                        Title = "New user",
                        UserId = User.GetUserId()
                    };
                    await _announcementHubContext.Clients.All.BroadcastAnnouncement(announcement);
                }
                else
                {
                    return new BadRequestObjectResult(result.Errors);
                }
            }
            else
            {
                var user = await _userManager.FindByIdAsync(appUser.Id.ToString());
                // remove current roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (appUser.Roles != null)
                {
                    var result = await _userManager.AddToRolesAsync(user, appUser.Roles.Except(currentRoles).ToArray());
                    if (result.Succeeded)
                    {
                        string[] needRemoveRoles = currentRoles.Except(appUser.Roles).ToArray();
                        await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);
                    }
                }

                user.FullName = appUser.FullName;
                user.Email = appUser.Email;
                user.Avatar = appUser.Avatar;
                user.PhoneNumber = appUser.PhoneNumber;
                user.BirthDay = DateTime.ParseExact(appUser.BirthDay, "dd/MM/yyyy", null);
                user.Status = appUser.Status;
                user.DateModified = DateTime.Now;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation($"User {user.UserName} info has been updated!");
                // update session
                var session = new AppUserViewModel()
                {
                    Id = appUser.Id,
                    FullName = appUser.FullName,
                    Avatar = appUser.Avatar,
                    UserName = appUser.UserName,
                };
                HttpContext.Session.Set("LoginSession", session);
            }
            return new OkObjectResult(appUser);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return new RedirectResult("/Admin/Login/Index");
                }

                var result = await _userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordSuccess");
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                return new OkObjectResult(id);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    x.FullName.Contains(keyword) || x.UserName.Contains(keyword) || x.Email.Contains(keyword));
            }

            int totalRow = await query.CountAsync();
            query = query.Skip((page - 1) * page).Take(pageSize);
            var data = await query.Select(x => new AppUserViewModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                Avatar = x.Avatar,
                BirthDay = x.BirthDay.ToString(),
                Email = x.Email,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated
            }).ToListAsync();
            return new OkObjectResult(new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = _mapper.Map<AppUser, AppUserViewModel>(user);
            userVm.Roles = roles.ToList();
            return new OkObjectResult(userVm);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allUser = _mapper.Map<IReadOnlyList<AppUser>, IReadOnlyList<AppUserViewModel>>(await _userManager.Users.ToListAsync());
            return new OkObjectResult(allUser);
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            if (!result.Succeeded)
            {
                return new RedirectResult("/Admin/Login/Index");
            }
            return View();
        }
    }
}