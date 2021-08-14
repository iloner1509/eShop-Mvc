using System;
using System.Globalization;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Specifications.BillSpecification
{
    public class BillWithFilterSpecification : BaseSpecification<Bill>
    {
        public BillWithFilterSpecification(BillPagingParams pagingParams) : base(x => (string.IsNullOrEmpty(pagingParams.SearchKeyword)
              || (x.CustomerName.ToLower().Contains(pagingParams.SearchKeyword) || x.CustomerAddress.ToLower().Contains(pagingParams.SearchKeyword) || x.CustomerMobile.Contains(pagingParams.SearchKeyword)))
              && (string.IsNullOrEmpty(pagingParams.StartDate) || x.DateCreated > DateTime.ParseExact(pagingParams.StartDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN")))
          && (string.IsNullOrEmpty(pagingParams.EndDate) || x.DateCreated < DateTime.ParseExact(pagingParams.EndDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"))))
        {
            ApplyPaging(pagingParams.PageSize, pagingParams.PageSize * (pagingParams.PageIndex - 1));
            switch (pagingParams.Sort)
            {
                case SortTypes.CustomerNameAsc:
                    AddOrderBy(x => x.CustomerName);
                    break;

                case SortTypes.CustomerNameDesc:
                    AddOrderByDesc(x => x.CustomerName);
                    break;

                case SortTypes.DateCreatedDesc:
                    AddOrderByDesc(x => x.DateCreated);
                    break;

                default:
                    AddOrderBy(x => x.DateCreated);
                    break;
            }
        }
    }
}