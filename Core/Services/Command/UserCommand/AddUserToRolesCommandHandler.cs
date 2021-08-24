using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Services.Command.UserCommand
{
    public class AddUserToRolesCommandHandler : IRequestHandler<AddUserToRolesCommand, IdentityResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public AddUserToRolesCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(AddUserToRolesCommand request, CancellationToken cancellationToken)
        {
            return await _userManager.AddToRolesAsync(request.User, request.Roles);
        }
    }
}