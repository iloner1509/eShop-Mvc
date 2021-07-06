﻿using System;
using eShop_Mvc.SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Entities
{
    public class AdvertisementPage : BaseEntity<string>, IAuditable
    {
        [StringLength(250)]
        public string Name { get; set; }

        public virtual ICollection<AdvertisementPosition> AdvertisementPositions { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
    }
}