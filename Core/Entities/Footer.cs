using System;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Entities
{
    public class Footer : BaseEntity<string>, IAuditable, IIpTracking
    {
        [Required]
        public string Content { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        [StringLength(30)]
        public string IpAddress { get; set; }
    }
}