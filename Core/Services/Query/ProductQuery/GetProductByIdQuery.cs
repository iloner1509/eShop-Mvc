using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int ProductId { get; set; }
    }
}