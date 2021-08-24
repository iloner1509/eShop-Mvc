using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Services.Command.UserCommand
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IdentityResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userManager.CreateAsync(request.AppUser, request.Password);
        }
    }
}