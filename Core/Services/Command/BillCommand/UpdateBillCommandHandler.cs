using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.BillCommand
{
    public class UpdateBillCommandHandler : IRequestHandler<UpdateBillCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBillCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
        {
            var billDetailRepo = _unitOfWork.Repository<BillDetail, int>();
            var productRepo = _unitOfWork.Repository<Product, int>();
            // get all order details
            var orderDetails = request.Bill.BillDetails;

            // get newly added details
            var addedDetails = orderDetails.Where(x => x.Id == 0).ToList();

            // get updated details
            var updatedDetails = orderDetails.Where(x => x.Id != 0).ToList();

            // get existed details
            var getExistedDetailSpecification = new BillDetailsByBillIdSpecification(request.Bill.Id);
            var existedDetails = await billDetailRepo.FindAllAsync(cancellationToken, getExistedDetailSpecification);
            //var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == request.Bill.Id);

            // clear all detail
            request.Bill.BillDetails.Clear();
            foreach (var detail in updatedDetails)
            {
                //var product = await _productRepository.FindByIdAsync(detail.ProductId);
                var product = await productRepo.FindByIdAsync(detail.ProductId);
                detail.Price = product.Price;
                billDetailRepo.Update(detail);
            }
            foreach (var detail in addedDetails)
            {
                var product = await productRepo.FindByIdAsync(detail.ProductId);
                detail.Price = product.Price;
                await billDetailRepo.AddAsync(detail, cancellationToken);
            }
            // delete old details
            var needDeleteDetail = existedDetails.AsEnumerable().Except(updatedDetails);
            billDetailRepo.DeleteRange(needDeleteDetail);

            _unitOfWork.Repository<Bill, int>().Update(request.Bill);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}