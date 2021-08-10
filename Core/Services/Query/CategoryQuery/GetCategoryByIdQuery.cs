using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.CategoryQuery
{
    public class GetCategoryByIdQuery : IRequest<ProductCategory>
    {
        public int CategoryId { get; set; }
    }
}