using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.BillCommand
{
    public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBillCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {
            foreach (var billDetail in request.Bill.BillDetails)
            {
                var product = await _unitOfWork.Repository<Product, int>().FindByIdAsync(billDetail.ProductId);
                billDetail.Price = product.Price;
            }

            await _unitOfWork.Repository<Bill, int>().AddAsync(request.Bill, cancellationToken: cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}