using eShop_Mvc.SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class AdvertisementPosition : BaseEntity<string>
    {
        public string PageId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public virtual AdvertisementPage AdvertisementPage { get; set; }

        public virtual IList<Advertisement> Advertisements { get; set; }
    }
}