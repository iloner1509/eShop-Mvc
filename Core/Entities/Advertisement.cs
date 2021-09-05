using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class Advertisement : BaseEntity<int>, ISwitchable, ISortable, IAuditable, IIpTracking
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        public string PositionId { get; set; }

        [Required]
        public Status Status { get; set; }

        public int SortOrder { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        [StringLength(30)]
        public string IpAddress { get; set; }

        public virtual AdvertisementPosition AdvertisementPosition { get; set; }
    }
}