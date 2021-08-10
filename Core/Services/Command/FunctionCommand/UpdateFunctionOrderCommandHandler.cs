using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class UpdateFunctionOrderCommandHandler : IRequestHandler<UpdateFunctionOrderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFunctionOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateFunctionOrderCommand request, CancellationToken cancellationToken)
        {
            var functionRepo = _unitOfWork.Repository<Function, string>();
            var source = await functionRepo.FindByIdAsync(request.SourceId);
            var target = await functionRepo.FindByIdAsync(request.TargetId);

            // Swap
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            functionRepo.Update(source);
            functionRepo.Update(target);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}