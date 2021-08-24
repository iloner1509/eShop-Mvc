using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class Blog : BaseEntity<int>, ISwitchable, IAuditable, IHasSeoMetaData
    {
        public Blog()
        {
        }

        public Blog(string name, string image, string description, string content, bool? homeFlag, bool hotFlag,
                    int? viewCount, string tags, Status status,
                    string seoTitle, string seoAlias, string seoKeywords, string seoDescription)
        {
            Name = name;
            Image = image;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            ViewCount = viewCount;
            Tags = tags;
            Status = status;
            SeoTitle = seoTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeywords;
            SeoDescription = seoDescription;
        }

        public Blog(int id, string name, string image, string description, string content, bool? homeFlag, bool hotFlag,
                    int? viewCount, string tags, Status status, string seoTitle, string seoAlias,
                    string seoKeywords, string seoDescription)
        {
            Id = id;
            Name = name;
            Image = image;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            ViewCount = viewCount;
            Tags = tags;
            Status = status;
            SeoTitle = seoTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeywords;
            SeoDescription = seoDescription;
        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public string Content { get; set; }
        public bool? HomeFlag { get; set; }
        public bool HotFlag { get; set; }
        public int? ViewCount { get; set; }
        public string Tags { get; set; }
        public virtual ICollection<BlogTag> BlogTags { get; set; }

        [Required]
        public Status Status { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
    }
}