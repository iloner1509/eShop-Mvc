using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.CategoryQuery
{
    public class GetHomeCategoryQuery : IRequest<IReadOnlyList<ProductCategory>>
    {
        public int Top { get; set; }
    }
}