using eShop_Mvc.Core.Entities;
using MediatR;
using System.Collections.Generic;

namespace eShop_Mvc.Core.Services.Query.CategoryQuery
{
    public class GetAllByParentIdQuery : IRequest<IReadOnlyList<ProductCategory>>
    {
        public int ParentId { get; set; }
    }
}