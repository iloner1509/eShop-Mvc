using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Specifications.BillSpecification
{
    public class BillPagingParams : BasePagingParams, IDateFilter, ISortFilter
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public SortTypes Sort { get; set; }
    }
}