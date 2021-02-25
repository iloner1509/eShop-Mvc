using System;

namespace eShop_Mvc.Models.System
{
    public class AnnouncementUserViewModel
    {
        public int Id { get; set; }
        public string AnnouncementId { get; set; }
        public Guid UserId { get; set; }
        public bool? HasRead { get; set; }
    }
}