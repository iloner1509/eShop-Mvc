using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services.Query.BillQuery
{
    public class GetListBillDetailByIdQueryHandler : IRequestHandler<GetListBillDetailByIdQuery, IReadOnlyList<BillDetail>>
    {
        private readonly IRepository<BillDetail, int> _orderDetailRepository;

        public GetListBillDetailByIdQueryHandler(IRepository<BillDetail, int> orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IReadOnlyList<BillDetail>> Handle(GetListBillDetailByIdQuery request, CancellationToken cancellationToken)
        {
            return await _orderDetailRepository.FindAll(od => od.BillId == request.BillId, p => p.Product)
                .ToListAsync(cancellationToken);
        }
    }
}