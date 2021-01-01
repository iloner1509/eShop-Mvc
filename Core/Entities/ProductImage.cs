using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class ProductImage : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [StringLength(250)]
        public string Path { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }
    }
}