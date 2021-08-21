using eShop_Mvc.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.RoleQuery
{
    public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, IReadOnlyList<AppRole>>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public GetAllRoleQueryHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IReadOnlyList<AppRole>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken) => await _roleManager.Roles.ToListAsync(cancellationToken);
    }
}