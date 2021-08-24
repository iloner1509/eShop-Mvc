using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Services.Query.UserQuery
{
    public class GetRolesByUserQueryHandler : IRequestHandler<GetRolesByUserQuery, IEnumerable<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetRolesByUserQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<string>> Handle(GetRolesByUserQuery request, CancellationToken cancellationToken)
        {
            return await _userManager.GetRolesAsync(request.User);
        }
    }
}