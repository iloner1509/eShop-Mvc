using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;
using eShop_Mvc.Core.Specifications.FunctionSpecification;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class UpdateFunctionParentIdCommandHandler : IRequestHandler<UpdateFunctionParentIdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFunctionParentIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateFunctionParentIdCommand request, CancellationToken cancellationToken)
        {
            var functionRepo = _unitOfWork.Repository<Function, string>();
            var sourceFunction = await functionRepo.FindByIdAsync(request.SourceId);
            sourceFunction.ParentId = request.TargetId;
            functionRepo.Update(sourceFunction);

            var specification = new FunctionByListFunctionIdSpecification(request.SubFunctionData.Keys);
            var sibling = await _unitOfWork.Repository<Function, string>().FindAllAsync(cancellationToken, specification);
            foreach (var item in sibling)
            {
                item.SortOrder = request.SubFunctionData[item.Id];
                functionRepo.Update(item);
            }
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}