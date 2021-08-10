using System;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Areas.Admin.Models
{
    public class AnnouncementViewModel
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string TimeStamp { get; set; }
    }
}