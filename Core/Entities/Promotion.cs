using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Entities
{
    public class Promotion : BaseEntity<string>, IAuditable, ISwitchable, IIpTracking
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public double? DiscountPercent { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountValue { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }
        public Status Status { get; set; } = Status.Active;

        [StringLength(30)]
        public string IpAddress { get; set; }

        public ICollection<PromotionProduct> PromotionProducts { get; set; }
    }
}