using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Models.System
{
    public class FunctionViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string URL { get; set; }

        public string ParentId { get; set; }
        public string IconCss { get; set; }

        [Required]
        public Status Status { get; set; }

        public int SortOrder { get; set; }
    }
}