using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Command
{
    public class UpdateBillStatusCommandHandler : IRequestHandler<UpdateBillStatusCommand>
    {
        private readonly IRepository<Bill, int> _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBillStatusCommandHandler(IRepository<Bill, int> orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateBillStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.FindByIdAsync(request.BillId);
            order.BillStatus = request.Status;
            await _orderRepository.UpdateAsync(order);
            _unitOfWork.Commit();
            return Unit.Value;
        }
    }
}