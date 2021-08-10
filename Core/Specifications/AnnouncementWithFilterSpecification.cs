using System;
using System.Globalization;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Specifications
{
    public class AnnouncementWithFilterSpecification : BaseSpecification<Announcement>
    {
        public AnnouncementWithFilterSpecification(PagingParams pagingParams) : base(x => (string.IsNullOrEmpty(pagingParams.SearchKeyword)
              || (x.Title.ToLower().Contains(pagingParams.SearchKeyword) || x.CreatedBy.ToLower().Contains(pagingParams.SearchKeyword)))
              && (string.IsNullOrEmpty(pagingParams.StartDate) || x.DateCreated > DateTime.ParseExact(pagingParams.StartDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN")))
              && (string.IsNullOrEmpty(pagingParams.EndDate) || x.DateCreated < DateTime.ParseExact(pagingParams.EndDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"))))
        {
            ApplyPaging(pagingParams.PageSize, pagingParams.PageSize * (pagingParams.PageIndex - 1));
            switch (pagingParams.Sort)
            {
                case SortTypes.TitleAsc:
                    AddOrderBy(x => x.Title);
                    break;

                case SortTypes.TitleDesc:
                    AddOrderByDesc(x => x.Title);
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