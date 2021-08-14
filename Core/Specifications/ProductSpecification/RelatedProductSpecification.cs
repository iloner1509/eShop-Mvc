using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Specifications.ProductSpecification
{
    public class RelatedProductSpecification : BaseSpecification<Product>
    {
        public RelatedProductSpecification(int productId, int categoryId, int top) : base(x => x.Id != productId && x.Status == Status.Active && x.CategoryId == categoryId)
        {
            AddOrderByDesc(x => x.DateCreated);
            AddTake(top);
        }
    }
}