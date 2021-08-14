using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.SharedKernel.Interfaces
{
    public interface ISortFilter
    {
        public SortTypes Sort { get; set; }
    }
}