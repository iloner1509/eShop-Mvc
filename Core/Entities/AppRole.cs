using System;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Entities
{
    public class AppRole : IdentityRole<Guid>, IAuditable
    {
        public AppRole()
        {
        }

        public AppRole(string name, string description, DateTime dateCreated, string createdBy) : base(name)
        {
            Description = description;
            DateCreated = dateCreated;
            CreatedBy = createdBy;
        }

        [StringLength(250)]
        public string Description { get; set; }

        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
    }
}