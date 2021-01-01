﻿using System;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class AnnouncementUser : BaseEntity<int>
    {
        public AnnouncementUser()
        {
        }

        public AnnouncementUser(string announcementId, Guid userId, bool? hasRead)
        {
            AnnouncementId = announcementId;
            UserId = userId;
            HasRead = hasRead;
        }

        [StringLength(100)]
        public string AnnouncementId { get; set; }

        public Guid UserId { get; set; }
        public bool? HasRead { get; set; }
        public virtual Announcement Announcement { get; set; }
    }
}