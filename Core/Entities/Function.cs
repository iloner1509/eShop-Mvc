using System;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class Function : BaseEntity<string>, ISwitchable, ISortable, IAuditable, IIpTracking
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string URL { get; set; }

        public string ParentId { get; set; }
        public string IconCss { get; set; }

        [Required]
        public Status Status { get; set; }

        public int SortOrder { get; set; }

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