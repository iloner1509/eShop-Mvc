using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class WholePrice : BaseEntity<int>
    {
        [Required]
        public int ProductId { get; set; }

        public int FromQuantity { get; set; }
        public int ToQuantity { get; set; }
        public decimal Price { get; set; }
        public virtual Product Product { get; set; }
    }
}