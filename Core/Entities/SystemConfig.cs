using System;
using System.ComponentModel.DataAnnotations;
using eShop_Mvc.Core.Enums;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class SystemConfig : BaseEntity<int>, ISwitchable
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Value1 { get; set; }
        public int? Value2 { get; set; }
        public bool? Value3 { get; set; }
        public DateTime? Value4 { get; set; }

        public decimal? Value5 { get; set; }

        public Status Status { get; set; }
    }
}