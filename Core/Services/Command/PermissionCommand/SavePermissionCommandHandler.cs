using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.PermissionSpecification;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Command.PermissionCommand
{
    public class SavePermissionCommandHandler : IRequestHandler<SavePermissionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SavePermissionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SavePermissionCommand request, CancellationToken cancellationToken)
        {
            var permissionRepo = _unitOfWork.Repository<Permission, int>();
            var spec = new PermissionByRoleIdSpecification(request.RoleId);
            var oldPermission = await permissionRepo.FindAllAsync(cancellationToken, spec);
            if (oldPermission.Count > 0)
            {
                permissionRepo.DeleteRange(oldPermission);
            }

            await permissionRepo.AddRangeAsync(request.ListPermission, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}