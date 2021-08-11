using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications
{
    public class ProductTagByProductIdSpecification : BaseSpecification<ProductTag>
    {
        public ProductTagByProductIdSpecification(int productId) : base(x => x.ProductId == productId)
        {
        }
    }
}