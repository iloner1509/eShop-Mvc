using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Command.RoleCommand
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public CreateRoleCommandHandler(IUnitOfWork unitOfWork, IUserService userService, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
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

            var result = await _roleManager.CreateAsync(new AppRole()
            {
                Name = request.Role.Name,
                Description = request.Role.Description,
                CreatedBy = _userService.GetUser().FindFirstValue(ClaimTypes.Name)
            });
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