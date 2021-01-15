using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class WholePrice : BaseEntity<int>
    {
        [Required]
        public int ProductId { get; set; }

        public int FromQuantity { get; set; }
        public int ToQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public virtual Product Product { get; set; }
    }
}