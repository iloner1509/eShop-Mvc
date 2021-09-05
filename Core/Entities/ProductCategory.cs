using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class ProductCategory : BaseEntity<int>, IHasSeoMetaData, ISwitchable, ISortable, IAuditable, IIpTracking
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? ParentId { get; set; }
        public int? HomeOrder { get; set; }
        public string Image { get; set; }
        public bool? HomeFlag { get; set; }
        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }

        [Required]
        public Status Status { get; set; }

        public int SortOrder { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        [StringLength(30)]
        public string IpAddress { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}