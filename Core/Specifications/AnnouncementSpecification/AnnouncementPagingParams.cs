using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Specifications.AnnouncementSpecification
{
    public class AnnouncementPagingParams : BasePagingParams, IDateFilter, ISortFilter
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public SortTypes Sort { get; set; }
    }
}