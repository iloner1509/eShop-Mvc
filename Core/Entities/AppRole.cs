using System;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Entities
{
    public class AppRole : IdentityRole<Guid>, IAuditable, IIpTracking
    {
        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        [StringLength(30)]
        public string IpAddress { get; set; }
    }
}