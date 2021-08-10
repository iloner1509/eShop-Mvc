using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Specifications
{
    public class PagingParams
    {
        private int _pageSize;
        private string _searchKeyword;
        private const int MaxPageSize = 50;
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string SearchKeyword
        {
            get => _searchKeyword;
            set => _searchKeyword = value.ToLower();
        }

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public SortTypes Sort { get; set; }
    }
}