using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.BillQuery
{
    public class GetListBillDetailByBillIdQueryHandler : IRequestHandler<GetListBillDetailByBillIdQuery, IReadOnlyList<BillDetail>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetListBillDetailByBillIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<BillDetail>> Handle(GetListBillDetailByBillIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new BillDetailsIncludeProductByBillIdSpecification(request.BillId);
            return await _unitOfWork.Repository<BillDetail, int>().FindAllAsync(cancellationToken, specification);
        }
    }
}