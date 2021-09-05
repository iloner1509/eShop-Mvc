using System;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Entities
{
    public class ProductImage : BaseEntity<int>, IAuditable
    {
        [StringLength(250)]
        public string Path { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }

        public int SortOrder { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}