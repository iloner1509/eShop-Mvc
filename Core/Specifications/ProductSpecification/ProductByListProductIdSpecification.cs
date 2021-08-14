using System.Collections.Generic;
using System.Linq;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications.ProductSpecification
{
    public class ProductByListProductIdSpecification : BaseSpecification<Product>
    {
        public ProductByListProductIdSpecification(IEnumerable<int> listProductId) : base(x => listProductId.Contains(x.Id))
        {
            AddOrderByDesc(x => x.PromotionPrice);
        }
    }
}