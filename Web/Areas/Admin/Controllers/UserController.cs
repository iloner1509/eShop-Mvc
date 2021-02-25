using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Models.AccountViewModels;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
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
                if (result.Succeeded && appUser.Roles.Count > 0)
                {
                    var createdUser = await _userManager.FindByNameAsync(user.UserName);
                    if (createdUser != null)
                    {
                        await _userManager.AddToRolesAsync(createdUser, appUser.Roles);
                    }
                }
            }
            else
            {
                var user = await _userManager.FindByIdAsync(appUser.Id.ToString());
                // remove current roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.AddToRolesAsync(user, appUser.Roles.Except(currentRoles).ToArray());
                if (result.Succeeded)
                {
                    string[] needRemoveRoles = currentRoles.Except(appUser.Roles).ToArray();
                    await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);

                    // update user
                    user.FullName = appUser.FullName;
                    user.Email = appUser.Email;
                    user.Avatar = appUser.Avatar;
                    user.PhoneNumber = appUser.PhoneNumber;
                    user.Status = appUser.Status;
                    user.DateModified = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                }
            }
            return new OkObjectResult(appUser);
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

        public IActionResult Index()
        {
            return View();
        }
    }
}