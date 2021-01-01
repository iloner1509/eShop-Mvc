using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShop_Mvc.Core.Enums;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Advertisement : BaseEntity<int>, ISwitchable, ISortable, IDateTracking
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

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public virtual AdvertisementPosition AdvertisementPosition { get; set; }
    }
}