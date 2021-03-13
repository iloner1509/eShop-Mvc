using eShop_Mvc.Models.ProductViewModels;

namespace eShop_Mvc.Models
{
    public class CartViewModel
    {
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}