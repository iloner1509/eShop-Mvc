using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.UserQuery
{
    public class GetRolesByUserQuery : IRequest<IEnumerable<string>>
    {
        public AppUser User { get; set; }
    }
}