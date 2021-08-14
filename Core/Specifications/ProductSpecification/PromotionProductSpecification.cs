using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Specifications.ProductSpecification
{
    public class PromotionProductSpecification : BaseSpecification<Product>
    {
        public PromotionProductSpecification(int top) : base(x => x.Status == Status.Active && x.PromotionPrice.HasValue)
        {
            AddOrderBy(x => x.DateCreated);
            AddTake(top);
        }
    }
}