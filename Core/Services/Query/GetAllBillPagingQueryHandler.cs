using System;
using System.Globalization;
using System.Linq;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services.Query
{
    public class GetAllBillPagingQueryHandler : IRequestHandler<GetAllBillPagingQuery, PagedResult<Bill>>
    {
        private readonly IRepository<Bill, int> _orderRepository;

        public GetAllBillPagingQueryHandler(IRepository<Bill, int> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PagedResult<Bill>> Handle(GetAllBillPagingQuery request, CancellationToken cancellationToken)
        {
            var query = _orderRepository.FindAll();
            if (!string.IsNullOrEmpty(request.StartDate))
            {
                DateTime start = DateTime.ParseExact(request.StartDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(b => b.DateCreated >= start);
            }

            if (!string.IsNullOrEmpty(request.EndDate))
            {
                DateTime end = DateTime.ParseExact(request.EndDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(b => b.DateCreated <= end);
            }

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(b => b.CustomerName.Contains(request.Keyword) || b.CustomerMobile.Contains(request.Keyword) || b.CustomerAddress.Contains(request.Keyword));
            }

            var totalRow = query.Count();
            var data = await query
                .OrderByDescending(b => b.DateCreated)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new PagedResult<Bill>()
            {
                CurrentPage = request.Page,
                RowCount = totalRow,
                Results = data,
                PageSize = request.PageSize
            };
        }
    }
}