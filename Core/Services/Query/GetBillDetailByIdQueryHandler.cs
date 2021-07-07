using eShop_Mvc.Core.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services.Query
{
    public class GetBillDetailByIdQueryHandler : IRequestHandler<GetBillDetailByIdQuery, Bill>
    {
        private readonly IRepository<Bill, int> _orderRepository;
        private readonly IRepository<BillDetail, int> _orderDetailRepository;

        public GetBillDetailByIdQueryHandler(IRepository<Bill, int> orderRepository, IRepository<BillDetail, int> orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<Bill> Handle(GetBillDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var bill = await _orderRepository.FindSingleAsync(b => b.Id == request.BillId);
            var billDetails = await _orderDetailRepository.FindAll(od => od.BillId == request.BillId).ToListAsync(cancellationToken: cancellationToken);
            bill.BillDetails = billDetails;
            return bill;
        }
    }
}