using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.CategoryQuery
{
    public class GetAllCategoryQuery : IRequest<IReadOnlyList<ProductCategory>>
    {
        public string? Keyword { get; set; }
    }
}