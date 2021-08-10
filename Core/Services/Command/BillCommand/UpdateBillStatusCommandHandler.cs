using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.BillCommand
{
    public class UpdateBillStatusCommandHandler : IRequestHandler<UpdateBillStatusCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBillStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateBillStatusCommand request, CancellationToken cancellationToken)
        {
            var billRepo = _unitOfWork.Repository<Bill, int>();

            var order = await billRepo.FindByIdAsync(request.BillId);
            order.BillStatus = request.Status;
            billRepo.Update(order);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}