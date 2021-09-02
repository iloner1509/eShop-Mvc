using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop_Mvc.Core.Entities
{
    public class Feedback : BaseEntity<int>, ISwitchable, IAuditable
    {
        [StringLength(150)]
        [Required]
        public string Name { get; set; }

        [MaxLength(150)]
        [Column(TypeName = "varchar(150)")]
        public string Email { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Active;

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }
    }
}