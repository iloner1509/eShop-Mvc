using System;
using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Models.ProductViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal? PromotionPrice { get; set; }

        public int CategoryId { get; set; }

        public decimal OriginalPrice { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }
        public bool? HomeFlag { get; set; }
        public bool? HotFlag { get; set; }
        public int? ViewCount { get; set; }

        public string Tags { get; set; }

        public string Unit { get; set; }

        public Status Status { get; set; }

        public string SeoTitle { get; set; }

        public string SeoAlias { get; set; }

        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
        public ICollection<BillDetail> BillDetails { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<WholePrice> WholePrices { get; set; }
    }
}