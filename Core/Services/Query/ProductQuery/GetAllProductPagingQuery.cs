using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.ProductSpecification;
using eShop_Mvc.SharedKernel;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetAllProductPagingQuery : IRequest<PagedResult<Product>>
    {
        public ProductPagingParams PagingParams { get; set; }
    }
}