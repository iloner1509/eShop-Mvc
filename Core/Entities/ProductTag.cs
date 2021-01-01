using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class ProductTag : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string TagId { get; set; }

        public Tag Tag { get; set; }
    }
}