using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop_Mvc.Core.Entities
{
    public class Currency : BaseEntity<int>, IAuditable, ISwitchable
    {
        [StringLength(100)]
        [Required]
        public string CurrencyName { get; set; }

        [Column(TypeName = "varchar(3)")]
        [Required]
        public string CurrencyCode { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}