using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop_Mvc.Core.Entities
{
    public class Product : BaseEntity<int>, ISwitchable, IHasSeoMetaData, IAuditable, IIpTracking
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        public string Image { get; set; }

        [Required]
        [DefaultValue(0)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PromotionPrice { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public string Content { get; set; }
        public bool? HomeFlag { get; set; }
        public bool? HotFlag { get; set; }
        public int? ViewCount { get; set; }

        [StringLength(200)]
        public string Tags { get; set; }

        public string Unit { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Active;

        [StringLength(250)]
        public string SeoTitle { get; set; }

        [StringLength(250)]
        public string SeoAlias { get; set; }

        [StringLength(250)]
        public string SeoKeywords { get; set; }

        [StringLength(250)]
        public string SeoDescription { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        [StringLength(30)]
        public string IpAddress { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<WholePrice> WholePrices { get; set; }
        public ICollection<PromotionProduct> PromotionProducts { get; set; }
    }
}