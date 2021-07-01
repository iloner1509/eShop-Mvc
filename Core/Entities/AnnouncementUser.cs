using eShop_Mvc.SharedKernel;
using System;

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

        public string AnnouncementId { get; set; }
        public Guid UserId { get; set; }
        public bool? HasRead { get; set; }
        public virtual Announcement Announcement { get; set; }
    }
}