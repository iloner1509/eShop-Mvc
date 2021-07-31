using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.BillCommand
{
    public class UpdateBillCommandHandler : IRequestHandler<UpdateBillCommand>
    {
        private readonly IRepository<Bill, int> _orderRepository;
        private readonly IRepository<BillDetail, int> _orderDetailRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBillCommandHandler(IRepository<Bill, int> orderRepository, IRepository<BillDetail, int> orderDetailRepository, IRepository<Product, int> productRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
        {
            // get all order details
            var orderDetails = request.Bill.BillDetails;

            // get newly added details
            var addedDetails = orderDetails.Where(x => x.Id == 0).ToList();

            // get updated details
            var updatedDetails = orderDetails.Where(x => x.Id != 0).ToList();

            // get existed details
            var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == request.Bill.Id);

            // clear all detail
            request.Bill.BillDetails.Clear();
            foreach (var detail in updatedDetails)
            {
                var product = await _productRepository.FindByIdAsync(detail.ProductId);
                detail.Price = product.Price;
                await _orderDetailRepository.UpdateAsync(detail);
            }
            foreach (var detail in addedDetails)
            {
                var product = await _productRepository.FindByIdAsync(detail.ProductId);
                detail.Price = product.Price;
                await _orderDetailRepository.AddAsync(detail);
            }
            // delete old details
            var needDeleteDetail = existedDetails.AsEnumerable().Except(updatedDetails);
            _orderDetailRepository.DeleteMultipleAsync(needDeleteDetail);
            await _orderRepository.UpdateAsync(request.Bill);
            _unitOfWork.Commit();
            return Unit.Value;
        }
    }
}