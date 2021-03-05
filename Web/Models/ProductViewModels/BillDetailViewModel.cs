namespace eShop_Mvc.Models.ProductViewModels
{
    public class BillDetailViewModel
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public BillViewModel Bill { get; set; }

        public ProductViewModel Product { get; set; }
    }
}