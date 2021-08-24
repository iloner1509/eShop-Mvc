using eShop_Mvc.Core.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Services.Query.UserQuery
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, AppUser>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserByIdQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userManager.FindByIdAsync(request.UserId);
        }
    }
}