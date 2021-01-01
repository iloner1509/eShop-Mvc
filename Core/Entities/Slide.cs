using System.ComponentModel.DataAnnotations;
using eShop_Mvc.Core.Enums;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Slide : BaseEntity<int>
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        public int? DisplayOrder { get; set; }

        [Required]
        public Status Status { get; set; }

        public string Content { get; set; }

        [Required]
        [StringLength(25)]
        public string GroupAlias { get; set; }
    }
}