﻿using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class ProductCategory : BaseEntity<int>, IHasSeoMetaData, ISwitchable, ISortable, IAuditable
    {
        public ProductCategory()
        {
            Products = new List<Product>();
        }

        public ProductCategory(string name, string description, int? parentId, int? homeOrder, string image,
                               bool? homeFlag, string seoTitle, string seoAlias, string seoKeywords,
                               string seoDescription, Status status, int sortOrder, DateTime dateCreated,
                               DateTime dateModified)
        {
            Name = name;
            Description = description;
            ParentId = parentId;
            HomeOrder = homeOrder;
            Image = image;
            HomeFlag = homeFlag;
            SeoTitle = seoTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeywords;
            SeoDescription = seoDescription;
            Status = status;
            SortOrder = sortOrder;
            DateCreated = dateCreated;
        }

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

        public string CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}