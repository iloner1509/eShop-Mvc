using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Specifications.BillSpecification;

namespace eShop_Mvc.Core.Services.Query.BillQuery
{
    public class GetAllBillPagingQueryHandler : IRequestHandler<GetAllBillPagingQuery, PagedResult<Bill>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBillPagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Bill>> Handle(GetAllBillPagingQuery request, CancellationToken cancellationToken)
        {
            var specification = new BillWithFilterSpecification(request.PagingParams);
            var totalRow = await _unitOfWork.Repository<Bill, int>().CountAsync(cancellationToken, specification);
            var data = await _unitOfWork.Repository<Bill, int>().FindAllAsync(cancellationToken, specification);
            return new PagedResult<Bill>()
            {
                CurrentPage = request.PagingParams.PageIndex,
                RowCount = totalRow,
                Results = data,
                PageSize = request.PagingParams.PageSize
            };
        }
    }
}