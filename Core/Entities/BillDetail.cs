using System.ComponentModel.DataAnnotations.Schema;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class BillDetail : BaseEntity<int>
    {
        public BillDetail()
        {
        }

        public BillDetail(int id, int billId, int productId, int quantity, decimal price)
        {
            Id = id;
            BillId = billId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public BillDetail(int billId, int productId, int quantity, decimal price)
        {
            BillId = billId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public int BillId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public virtual Bill Bill { get; set; }

        public virtual Product Product { get; set; }
    }
}