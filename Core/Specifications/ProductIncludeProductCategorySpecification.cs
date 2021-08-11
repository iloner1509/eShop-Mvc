using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications
{
    public class ProductIncludeProductCategorySpecification : BaseSpecification<Product>
    {
        public ProductIncludeProductCategorySpecification() : base()
        {
            AddInclude(x => x.ProductCategory);
        }
    }
}