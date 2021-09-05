using System;
using eShop_Mvc.SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Entities
{
    public class AdvertisementPosition : BaseEntity<string>, IAuditable, IIpTracking
    {
        public string PageId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public virtual AdvertisementPage AdvertisementPage { get; set; }

        public virtual IList<Advertisement> Advertisements { get; set; }

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