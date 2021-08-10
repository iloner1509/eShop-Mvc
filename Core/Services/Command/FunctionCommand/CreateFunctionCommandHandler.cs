using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class CreateFunctionCommandHandler : IRequestHandler<CreateFunctionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFunctionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateFunctionCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Repository<Function, string>().AddAsync(request.Function, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}