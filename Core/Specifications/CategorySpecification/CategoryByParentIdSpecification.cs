using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Specifications.CategorySpecification
{
    public class CategoryByParentIdSpecification : BaseSpecification<ProductCategory>
    {
        public CategoryByParentIdSpecification(int parentId) : base(x => x.Status == Status.Active && x.ParentId == parentId)
        {
            AddOrderBy(x => x.SortOrder);
        }
    }
}