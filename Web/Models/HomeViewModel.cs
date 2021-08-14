using eShop_Mvc.Models.BlogViewModels;
using eShop_Mvc.Models.ProductViewModels;
using System.Collections.Generic;
using eShop_Mvc.Models.Common;

namespace eShop_Mvc.Models
{
    public class HomeViewModel
    {
        public IReadOnlyList<BlogViewModel> LatestBlogs { get; set; }
        public IReadOnlyList<SlideViewModel> HomeSlides { get; set; }
        public IReadOnlyList<ProductViewModel> HotProducts { get; set; }
        public IReadOnlyList<ProductViewModel> TopSellProducts { get; set; }
        public IReadOnlyList<ProductViewModel> LatestProducts { get; set; }
        public IReadOnlyList<ProductCategoryViewModel> HomeCategories { get; set; }
        public string Title { get; set; }
    }
}