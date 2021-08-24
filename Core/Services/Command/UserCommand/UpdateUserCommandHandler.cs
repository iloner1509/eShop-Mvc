using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Services.Command.UserCommand
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IdentityResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public UpdateUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.AppUser.Id.ToString());
            // remove current roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (request.Roles != null && request.Roles.Any())
            {
                var result = await _userManager.AddToRolesAsync(user, request.Roles.Except(currentRoles).ToArray());
                if (result.Succeeded)
                {
                    string[] needRemoveRoles = currentRoles.Except(request.Roles).ToArray();
                    await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);
                }
                return IdentityResult.Failed(result.Errors.ToArray());
            }

            user.FullName = request.AppUser.FullName;
            user.Email = request.AppUser.Email;
            user.Avatar = request.AppUser.Avatar;
            user.PhoneNumber = request.AppUser.PhoneNumber;
            user.BirthDay = request.AppUser.BirthDay;
            user.Status = request.AppUser.Status;
            return await _userManager.UpdateAsync(user);
        }
    }
}