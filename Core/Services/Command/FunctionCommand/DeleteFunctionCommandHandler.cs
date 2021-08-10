using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class DeleteFunctionCommandHandler : IRequestHandler<DeleteFunctionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFunctionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteFunctionCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Repository<Function, string>().DeleteByIdAsync(request.FunctionId);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}