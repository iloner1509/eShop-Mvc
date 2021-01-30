using System;
using System.Collections.Generic;

namespace eShop_Mvc.SharedKernel
{
    public class PagedResult<T> where T : class
    {
        public PagedResult()
        {
            Results = new List<T>();
        }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        private int _pageCount;

        public int PageCount
        {
            get => _pageCount = (int)Math.Ceiling((double)RowCount / PageSize);

            set => _pageCount = value;
        }

        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;
        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
        public IReadOnlyList<T> Results { get; set; }
    }
}