using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.RoleQuery
{
    public class GetAllRolePagingQuery : IRequest<PagedResult<AppRole>>
    {
        public BasePagingParams PagingParams { get; set; }
    }
}