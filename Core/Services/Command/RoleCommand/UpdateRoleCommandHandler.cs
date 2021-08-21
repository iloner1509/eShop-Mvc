using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Services.Command.RoleCommand
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IUserService userService, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _userManager.GetUserAsync(_userService.GetUser());
            var listRoleOfCurrentUser = await _userManager.GetRolesAsync(currentUser);

            // Get list of user who need receive announcement
            IList<string> listUserId = new List<string>();
            foreach (var role in listRoleOfCurrentUser)
            {
                var listUserInRole = await _userManager.GetUsersInRoleAsync(role);
                foreach (var user in listUserInRole)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    if (!listUserId.Contains(userId))
                    {
                        listUserId.Add(userId);
                    }
                }
            }

            var listAnnouncementUser = listUserId.Select(userId => new AnnouncementUser()
            {
                AnnouncementId = request.Announcement.Id,
                HasRead = false,
                UserId = Guid.Parse(userId)
            }).ToList();

            var updateRole = await _roleManager.FindByIdAsync(request.Role.Id.ToString());
            updateRole.Name = request.Role.Name;
            updateRole.Description = request.Role.Description;
            updateRole.ModifiedBy = _userService.GetUser().FindFirstValue(ClaimTypes.Name);
            var result = await _roleManager.UpdateAsync(updateRole);
            if (result.Succeeded)
            {
                await _unitOfWork.Repository<Announcement, string>().AddAsync(request.Announcement, cancellationToken);
                await _unitOfWork.Repository<AnnouncementUser, int>().AddRangeAsync(listAnnouncementUser, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return true;
            }

            return false;
        }
    }
}