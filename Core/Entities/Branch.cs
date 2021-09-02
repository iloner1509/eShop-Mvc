using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Entities
{
    public class Branch : BaseEntity<string>, IAuditable, ISwitchable
    {
        [StringLength(100)]
        [Required]
        public string BranchName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public string Address { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public string ContactPerson { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}