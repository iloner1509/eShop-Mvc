using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop_Mvc.Core.Entities
{
    public class AppUser : IdentityUser<Guid>, IAuditable, ISwitchable, IIpTracking
    {
        [StringLength(150)]
        [Required]
        public string FullName { get; set; }

        public DateTime? BirthDay { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public string Avatar { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        [Required]
        public Status Status { get; set; }

        [StringLength(30)]
        public string IpAddress { get; set; }

        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}