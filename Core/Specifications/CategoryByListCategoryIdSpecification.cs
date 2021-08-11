using System.Collections.Generic;
using System.Linq;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications
{
    public class CategoryByListCategoryIdSpecification : BaseSpecification<ProductCategory>
    {
        public CategoryByListCategoryIdSpecification(IEnumerable<int> categoryId) : base(x => categoryId.Contains(x.Id))
        {
        }
    }
}