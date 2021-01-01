using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class AdvertisementPosition : BaseEntity<string>
    {
        [StringLength(20)]
        public string PageId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public virtual AdvertisementPage AdvertisementPage { get; set; }

        public virtual IList<Advertisement> Advertisements { get; set; }
    }
}