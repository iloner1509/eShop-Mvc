using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class Announcement : BaseEntity<string>, IAuditable
    {
        public Announcement()
        {
            AnnouncementUsers = new List<AnnouncementUser>();
        }

        public Announcement(string title, string content, Guid userId, string createdBy, DateTime dateCreated)
        {
            Title = title;
            Content = content;
            UserId = userId;
            CreatedBy = createdBy;
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

        [StringLength(250)]
        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
    }
}