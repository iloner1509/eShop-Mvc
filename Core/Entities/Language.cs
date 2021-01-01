using System.ComponentModel.DataAnnotations;
using eShop_Mvc.Core.Enums;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Language : BaseEntity<string>, ISwitchable
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsDefault { get; set; }
        public string Resources { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}