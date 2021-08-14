using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetLatestProductQuery : IRequest<IReadOnlyList<Product>>
    {
        public int Top { get; set; }
    }
}