using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.Core.Enums;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Announcement : BaseEntity<string>, ISwitchable, IDateTracking
    {
        public Announcement()
        {
            AnnouncementUsers = new List<AnnouncementUser>();
        }

        public Announcement(string title, string content, Guid userId, Status status)
        {
            Title = title;
            Content = content;
            UserId = userId;

            Status = status;
        }

        [StringLength(250)]
        [Required]
        public string Title { get; set; }

        [StringLength(250)]
        public string Content { get; set; }

        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<AnnouncementUser> AnnouncementUsers { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}