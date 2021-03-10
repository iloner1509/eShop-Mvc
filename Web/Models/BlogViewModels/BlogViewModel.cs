using System;
using System.Collections.Generic;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Models.BlogViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }
        public bool? HomeFlag { get; set; }
        public bool HotFlag { get; set; }
        public int? ViewCount { get; set; }
        public string Tags { get; set; }
        public virtual ICollection<BlogTagViewModel> BlogTags { get; set; }

        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
    }
}