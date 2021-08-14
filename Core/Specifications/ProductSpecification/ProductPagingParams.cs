using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Specifications.ProductSpecification
{
    public class ProductPagingParams : BasePagingParams, ISortFilter
    {
        public SortTypes Sort { get; set; }
        public int? CategoryId { get; set; }
    }
}