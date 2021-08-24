using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Services.Query.UserQuery
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, SignInResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public LoginQueryHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user != null)
            {
                return await _signInManager.PasswordSignInAsync(request.Username, request.Password, request.RememberMe, true);
            }
            return SignInResult.NotAllowed;
        }
    }
}