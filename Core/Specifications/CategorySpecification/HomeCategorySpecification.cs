using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications.CategorySpecification
{
    public class HomeCategorySpecification : BaseSpecification<ProductCategory>
    {
        public HomeCategorySpecification(int top) : base(c => c.HomeFlag == true)
        {
            AddInclude(c => c.Products);
            AddOrderBy(c => c.HomeOrder);
            AddTake(top);
        }
    }
}