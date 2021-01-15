using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;

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