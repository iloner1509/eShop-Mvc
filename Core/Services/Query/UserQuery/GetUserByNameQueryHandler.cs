using eShop_Mvc.Core.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Services.Query.UserQuery
{
    public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, AppUser>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserByNameQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            return await _userManager.FindByNameAsync(request.Username);
        }
    }
}