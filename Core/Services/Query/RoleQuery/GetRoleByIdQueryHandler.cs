using eShop_Mvc.Core.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Services.Query.RoleQuery
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, AppRole>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public GetRoleByIdQueryHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<AppRole> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleManager.FindByIdAsync(request.RoleId.ToString());
        }
    }
}