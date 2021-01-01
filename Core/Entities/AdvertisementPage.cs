using eShop_Mvc.SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class AdvertisementPage : BaseEntity<string>
    {
        [StringLength(250)]
        public string Name { get; set; }

        public virtual ICollection<AdvertisementPosition> AdvertisementPositions { get; set; }
    }
}