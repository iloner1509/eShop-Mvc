using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications.CategorySpecification
{
    public class ProductCategoryWithFilterSpecification : BaseSpecification<ProductCategory>
    {
        public ProductCategoryWithFilterSpecification(string keyword) : base(x => (string.IsNullOrEmpty(keyword) || x.Name.ToLower().Contains(keyword)))
        {
            AddOrderBy(x => x.ParentId);
        }
    }
}