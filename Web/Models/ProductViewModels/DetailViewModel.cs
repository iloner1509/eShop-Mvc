using System.Collections.Generic;
using eShop_Mvc.Models.Common;

namespace eShop_Mvc.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public ProductViewModel Product { get; set; }
        public bool Available { get; set; }
        public ProductCategoryViewModel Category { get; set; }
        public IReadOnlyList<ProductViewModel> RelatedProducts { get; set; }

        public IReadOnlyList<TagViewModel> Tags { get; set; }
    }
}