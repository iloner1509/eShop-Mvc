using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.SharedKernel
{
    public class BasePagingParams
    {
        private int _pageSize;
        private string _searchKeyword;
        private const int MaxPageSize = 50;

        public string SearchKeyword
        {
            get => _searchKeyword;
            set => _searchKeyword = value?.ToLower();
        }

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}