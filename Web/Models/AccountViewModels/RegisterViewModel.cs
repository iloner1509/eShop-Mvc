using System;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [EmailAddress]
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}