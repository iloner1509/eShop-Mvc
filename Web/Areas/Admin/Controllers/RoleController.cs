using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Areas.Admin.Models;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Core.Services.Command.PermissionCommand;
using eShop_Mvc.Core.Services.Command.RoleCommand;
using eShop_Mvc.Core.Services.Query.PermissionQuery;
using eShop_Mvc.Core.Services.Query.RoleQuery;
using eShop_Mvc.Extensions;
using eShop_Mvc.Models.AccountViewModels;
using eShop_Mvc.Models.System;
using eShop_Mvc.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<RoleController> _logger;

        public IActionResult Index()
        {
            return View();
        }

        public RoleController(IMapper mapper, IMediator mediator, ILogger<RoleController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return new OkObjectResult(await _mediator.Send(new GetAllRoleQuery()));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return new OkObjectResult(await _mediator.Send(new GetRoleByIdQuery()
            {
                RoleId = id
            }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(BasePagingParams pagingParams)
        {
            var model = await _mediator.Send(new GetAllRolePagingQuery()
            {
                PagingParams = pagingParams
            });
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
                var announcement = new AnnouncementViewModel()
                {
                    Title = "New Role created",
                    TimeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    Content = $"Role {roleViewModel.Name} had been created !",
                    Id = Guid.NewGuid().ToString(),
                    UserId = User.GetUserId(),
                    CreatedBy = User.FindFirstValue(ClaimTypes.Name)
                };
                var result = await _mediator.Send(new CreateRoleCommand()
                {
                    Announcement = _mapper.Map<AnnouncementViewModel, Announcement>(announcement),
                    Role = _mapper.Map<AppRoleViewModel, AppRole>(roleViewModel)
                });
                if (!result)
                {
                    return new BadRequestResult();
                }
                _logger.LogInformation($"Role {roleViewModel.Name} had been created !");
            }
            else
            {
                var announcement = new AnnouncementViewModel()
                {
                    Title = "Role updated",
                    TimeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    Content = $"Role {roleViewModel.Name} had been modified !",
                    Id = Guid.NewGuid().ToString(),
                    UserId = User.GetUserId(),
                    CreatedBy = User.FindFirstValue(ClaimTypes.Name)
                };
                var result = await _mediator.Send(new UpdateRoleCommand()
                {
                    Announcement = _mapper.Map<AnnouncementViewModel, Announcement>(announcement),
                    Role = _mapper.Map<AppRoleViewModel, AppRole>(roleViewModel)
                });
                if (!result)
                {
                    return new BadRequestResult();
                }
                _logger.LogInformation($"Role {roleViewModel.Name} had been modified !");
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

            var result = await _mediator.Send(new DeleteRoleCommand()
            {
                RoleId = id
            });
            if (!result)
            {
                return new BadRequestResult();
            }

            return new OkObjectResult(id);
        }

        [HttpPost]
        public async Task<IActionResult> ListAllFunction(Guid roleId)
        {
            var permission = await _mediator.Send(new GetPermissionByRoleIdQuery()
            {
                RoleId = roleId
            });
            return new OkObjectResult(permission);
        }

        [HttpPost]
        public async Task<IActionResult> SavePermission(List<PermissionViewModel> listPermission, Guid roleId)
        {
            await _mediator.Send(new SavePermissionCommand()
            {
                ListPermission = _mapper.Map<List<PermissionViewModel>, List<Permission>>(listPermission),
                RoleId = roleId
            });
            _logger.LogInformation($"Permission for role: {roleId} had updated!");
            return new OkResult();
        }
    }
}