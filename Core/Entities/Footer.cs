using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Footer : BaseEntity<string>
    {
        [Required]
        public string Content { get; set; }
    }
}