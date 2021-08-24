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
    public class Product : BaseEntity<int>, ISwitchable, IHasSeoMetaData, IAuditable
    {
        public Product()
        {
        }

        public Product(string name, string image, int quantity, decimal price, decimal? promotionPrice, int categoryId,
                       decimal originalPrice, string description, string content, bool? homeFlag, bool? hotFlag,
                       int? viewCount, string tags, string unit, Status status, string seoTitle, string seoAlias,
                       string seoKeywords, string seoDescription)
        {
            Name = name;
            Image = image;
            Price = price;
            PromotionPrice = promotionPrice;
            CategoryId = categoryId;
            OriginalPrice = originalPrice;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            ViewCount = viewCount;
            Tags = tags;
            Unit = unit;
            Quantity = quantity;
            Status = status;
            SeoTitle = seoTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeywords;
            SeoDescription = seoDescription;
            ProductTags = new List<ProductTag>();
            ProductImages = new List<ProductImage>();
        }

        public Product(int id, string name, int quantity, string image, decimal price, decimal? promotionPrice,
                       int categoryId, decimal originalPrice, string description, string content, bool? homeFlag,
                       bool? hotFlag, int? viewCount, string tags, string unit, Status status, string seoTitle,
                       string seoAlias, string seoKeywords, string seoDescription)
        {
            Id = id;
            Name = name;
            Image = image;
            Price = price;
            PromotionPrice = promotionPrice;
            CategoryId = categoryId;
            OriginalPrice = originalPrice;
            Description = description;
            Content = content;
            Quantity = quantity;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            ViewCount = viewCount;
            Tags = tags;
            Unit = unit;
            Status = status;
            SeoTitle = seoTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeywords;
            SeoDescription = seoDescription;
            ProductTags = new List<ProductTag>();
            ProductImages = new List<ProductImage>();
        }

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

        [Required]
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
        public Status Status { get; set; }

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

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<WholePrice> WholePrices { get; set; }
    }
}