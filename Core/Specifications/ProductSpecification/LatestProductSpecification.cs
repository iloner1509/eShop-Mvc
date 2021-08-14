using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Specifications.ProductSpecification
{
    public class LatestProductSpecification : BaseSpecification<Product>
    {
        public LatestProductSpecification(int top) : base(x => x.Status == Status.Active)
        {
            AddOrderByDesc(x => x.DateCreated);
            AddTake(top);
        }
    }
}