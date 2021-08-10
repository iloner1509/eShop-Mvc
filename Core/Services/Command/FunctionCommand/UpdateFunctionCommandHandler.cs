using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class UpdateFunctionCommandHandler : IRequestHandler<UpdateFunctionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFunctionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateFunctionCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Repository<Function, string>().Update(request.Function);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}