using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.PermissionSpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.PermissionQuery
{
    public class GetPermissionByRoleIdQueryHandler : IRequestHandler<GetPermissionByRoleIdQuery, IReadOnlyList<Permission>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPermissionByRoleIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Permission>> Handle(GetPermissionByRoleIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new PermissionByRoleIdSpecification(request.RoleId);
            return await _unitOfWork.Repository<Permission, int>().FindAllAsync(cancellationToken, specification);
        }
    }
}