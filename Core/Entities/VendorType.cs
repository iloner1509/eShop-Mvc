using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Entities
{
    public class VendorType : BaseEntity<int>, IAuditable, ISwitchable
    {
        [StringLength(100)]
        [Required]
        public string VendorTypeName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public ICollection<Vendor> Vendors { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}