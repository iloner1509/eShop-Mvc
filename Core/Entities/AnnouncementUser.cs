﻿using eShop_Mvc.SharedKernel;
using System;

namespace eShop_Mvc.Core.Entities
{
    public class AnnouncementUser : BaseEntity<int>
    {
        public string AnnouncementId { get; set; }
        public Guid UserId { get; set; }
        public bool? HasRead { get; set; }
        public virtual Announcement Announcement { get; set; }
    }
}