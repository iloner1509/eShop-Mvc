using System;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Models.System
{
    public class AnnouncementViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }
        public Guid UserId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Status Status { get; set; }
    }
}