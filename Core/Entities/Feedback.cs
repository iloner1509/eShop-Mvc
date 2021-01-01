using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShop_Mvc.Core.Enums;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Feedback : BaseEntity<int>, ISwitchable, IDateTracking
    {
        public Feedback()
        {
        }

        public Feedback(int id, string name, string email, string message, Status status)
        {
            Id = id;
            Name = name;
            Email = email;
            Message = message;
            Status = status;
        }

        [StringLength(150)]
        [Required]
        public string Name { get; set; }

        [MaxLength(150)]
        [Column(TypeName = "varchar")]
        public string Email { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}