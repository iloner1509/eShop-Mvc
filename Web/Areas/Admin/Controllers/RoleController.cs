using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Extensions;
using eShop_Mvc.Models.AccountViewModels;
using eShop_Mvc.Models.System;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public IActionResult Index()
        {
            return View();
        }

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return new OkObjectResult(await _roleService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return new OkObjectResult(await _roleService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = await _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(new PagedResult<AppRoleViewModel>()
            {
                CurrentPage = model.CurrentPage,
                PageSize = model.PageSize,
                RowCount = model.RowCount,
                Results = _mapper.Map<IReadOnlyList<AppRole>, IReadOnlyList<AppRoleViewModel>>(model.Results)
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (!roleViewModel.Id.HasValue)
            {
                var announcementId = Guid.NewGuid().ToString();
                var announcement = new AnnouncementViewModel()
                {
                    Title = "Tạo mới role",
                    DateCreated = DateTime.Now,
                    Content = $"Role {roleViewModel.Name} đã được thêm mới !",
                    Id = announcementId,
                    UserId = User.GetUserId()
                };
                var announcementUser = new List<AnnouncementUserViewModel>()
                {
                    new AnnouncementUserViewModel()
                    {
                        AnnouncementId = announcementId,
                        HasRead = false,
                        UserId = User.GetUserId()
                    }
                };
                await _roleService.AddAsync(
                    _mapper.Map<AnnouncementViewModel, Announcement>(announcement),
                    _mapper.Map<List<AnnouncementUserViewModel>, List<AnnouncementUser>>(announcementUser),
                    _mapper.Map<AppRoleViewModel, AppRole>(roleViewModel));
            }
            else
            {
                await _roleService.UpdateAsync(_mapper.Map<AppRoleViewModel, AppRole>(roleViewModel));
            }
            return new OkObjectResult(roleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _roleService.DeleteAsync(id);
            return new OkObjectResult(id);
        }

        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            return new OkObjectResult(_roleService.GetListPermissionWithRole(roleId));
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermission, Guid roleId)
        {
            _roleService.SavePermission(
                _mapper.Map<List<PermissionViewModel>, List<Permission>>(listPermission),
                roleId);
            return new OkResult();
        }
    }
}