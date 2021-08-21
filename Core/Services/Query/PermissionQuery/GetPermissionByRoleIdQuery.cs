using System;
using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.PermissionQuery
{
    public class GetPermissionByRoleIdQuery : IRequest<IReadOnlyList<Permission>>
    {
        public Guid RoleId { get; set; }
    }
}