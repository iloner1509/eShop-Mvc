using System;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class PromotionProduct : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}