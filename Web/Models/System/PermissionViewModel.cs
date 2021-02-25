using System;
using eShop_Mvc.Models.AccountViewModels;

namespace eShop_Mvc.Models.System
{
    public class PermissionViewModel
    {
        public int Id { get; set; }
        public Guid RoleId { get; set; }
        public string FunctionId { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanCreate { get; set; }
        public bool CanDelete { get; set; }
        public AppRoleViewModel AppRole { get; set; }
        public FunctionViewModel Function { get; set; }
    }
}