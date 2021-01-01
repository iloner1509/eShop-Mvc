using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Tag : BaseEntity<string>
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string Type { get; set; }

        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<BlogTag> BlogTags { get; set; }
    }
}