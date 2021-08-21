using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.RoleQuery
{
    public class GetAllRolePagingQueryHandler : IRequestHandler<GetAllRolePagingQuery, PagedResult<AppRole>>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public GetAllRolePagingQueryHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<PagedResult<AppRole>> Handle(GetAllRolePagingQuery request, CancellationToken cancellationToken)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(request.PagingParams.SearchKeyword))
            {
                query = query.Where(x => x.Name.Contains(request.PagingParams.SearchKeyword) || x.Description.Contains(request.PagingParams.SearchKeyword));
            }

            int totalRow = query.Count();
            query = query.Skip((request.PagingParams.PageIndex - 1) * request.PagingParams.PageSize).Take(request.PagingParams.PageSize);
            var data = await query.ToListAsync(cancellationToken);
            return new PagedResult<AppRole>()
            {
                Results = data,
                CurrentPage = request.PagingParams.PageIndex,
                RowCount = totalRow,
                PageSize = request.PagingParams.PageSize
            };
        }
    }
}