using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop_Mvc.Core.Entities
{
    public class Feedback : BaseEntity<int>, ISwitchable, IAuditable
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
        [Column(TypeName = "varchar(150)")]
        public string Email { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        [Required]
        public Status Status { get; set; }

        public string CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
    }
}