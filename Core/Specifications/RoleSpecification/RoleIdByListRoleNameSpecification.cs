using System.Collections.Generic;
using System.Linq;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications.RoleSpecification
{
    public class RoleIdByListRoleNameSpecification : BaseSpecification<AppRole>
    {
        public RoleIdByListRoleNameSpecification(IEnumerable<string> listRoleName) : base(x => listRoleName.Contains(x.Name))
        {
            AddOrderBy(x => x.Name);
        }
    }
}