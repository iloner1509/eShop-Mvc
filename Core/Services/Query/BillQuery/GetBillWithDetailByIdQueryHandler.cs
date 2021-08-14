using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.BillSpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.BillQuery
{
    public class GetBillWithDetailByIdQueryHandler : IRequestHandler<GetBillWithDetailByIdQuery, Bill>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBillWithDetailByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Bill> Handle(GetBillWithDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var bill = await _unitOfWork.Repository<Bill, int>().FindByIdAsync(request.BillId);
            var specification = new BillDetailsByBillIdSpecification(request.BillId);
            var billDetails = await _unitOfWork.Repository<BillDetail, int>().FindAllAsync(cancellationToken: cancellationToken, specification);
            bill.BillDetails = (ICollection<BillDetail>)billDetails;
            return bill;
        }
    }
}