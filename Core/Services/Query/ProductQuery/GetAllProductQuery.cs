using eShop_Mvc.Core.Entities;
using MediatR;
using System.Collections.Generic;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetAllProductQuery : IRequest<IReadOnlyList<Product>>
    {
    }
}