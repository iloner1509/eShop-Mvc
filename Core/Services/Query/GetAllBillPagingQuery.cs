using System;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel;
using MediatR;

namespace eShop_Mvc.Core.Services.Query
{
    public class GetAllBillPagingQuery : IRequest<PagedResult<Bill>>
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
    }
}