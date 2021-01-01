using System;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Permission : BaseEntity<int>
    {
        public Permission()
        {
        }

        public Permission(Guid roleId, string functionId, bool canCreate, bool canRead, bool canUpdate, bool canDelete)
        {
            RoleId = roleId;
            FunctionId = functionId;
            CanCreate = canCreate;
            CanRead = canRead;
            CanUpdate = canUpdate;
            CanDelete = canDelete;
        }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        [StringLength(100)]
        public string FunctionId { get; set; }

        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public virtual AppRole Role { get; set; }
        public virtual Function Function { get; set; }
    }
}