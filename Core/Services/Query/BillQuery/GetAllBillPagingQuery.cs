using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.BillSpecification;
using eShop_Mvc.SharedKernel;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.BillQuery
{
    public class GetAllBillPagingQuery : IRequest<PagedResult<Bill>>
    {
        public BillPagingParams PagingParams { get; set; }
    }
}