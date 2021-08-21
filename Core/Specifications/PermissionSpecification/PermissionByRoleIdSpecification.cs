using System;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications.PermissionSpecification
{
    public class PermissionByRoleIdSpecification : BaseSpecification<Permission>
    {
        public PermissionByRoleIdSpecification(Guid roleId) : base(x => x.RoleId == roleId)
        {
        }
    }
}