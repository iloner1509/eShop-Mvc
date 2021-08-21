using System.Linq;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services.Query.PermissionQuery
{
    public class CheckPermissionQueryHandler : IRequestHandler<CheckPermissionQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<AppRole> _roleManager;

        public CheckPermissionQueryHandler(IUnitOfWork unitOfWork, RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(CheckPermissionQuery request, CancellationToken cancellationToken)
        {
            var permissions = _unitOfWork.Repository<Permission, int>().ApplySpecification();
            var query = from p in permissions
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where request.Roles.Contains(r.Name) && p.FunctionId == request.FunctionId && ((p.CanCreate && request.Action == "Create") ||
                            (p.CanUpdate && request.Action == "Update") ||
                            (p.CanDelete && request.Action == "Delete") ||
                            (p.CanRead && request.Action == "Read"))
                        select p;
            return await query.AnyAsync(cancellationToken);
        }
    }
}