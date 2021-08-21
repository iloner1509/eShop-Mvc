using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.RoleQuery
{
    public class GetAllRoleQuery : IRequest<IReadOnlyList<AppRole>>
    {
    }
}