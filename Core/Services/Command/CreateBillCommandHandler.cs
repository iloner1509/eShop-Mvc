using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Command
{
    public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand>
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Bill, int> _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBillCommandHandler(IRepository<Product, int> productRepository, IRepository<Bill, int> orderRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {
            foreach (var billDetail in request.Bill.BillDetails)
            {
                var product = await _productRepository.FindByIdAsync(billDetail.ProductId);
                billDetail.Price = product.Price;
            }

            await _orderRepository.AddAsync(request.Bill);
            _unitOfWork.Commit();
            return Unit.Value;
        }
    }
}