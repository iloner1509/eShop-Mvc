using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole()
        {
        }

        public AppRole(string name, string description) : base(name)
        {
            Description = description;
        }

        [StringLength(250)]
        public string Description { get; set; }
    }
}