using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetRelatedProductQuery : IRequest<IReadOnlyList<Product>>
    {
        public int ProductId { get; set; }
        public int Top { get; set; }
    }
}