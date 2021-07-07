using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class Announcement : BaseEntity<string>, ISwitchable, IAuditable
    {
        public Announcement()
        {
            AnnouncementUsers = new List<AnnouncementUser>();
        }

        public Announcement(string title, string content, Guid userId, Status status, DateTime dateCreated)
        {
            Title = title;
            Content = content;
            UserId = userId;
            Status = status;
            DateCreated = dateCreated;
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

        public string CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
    }
}