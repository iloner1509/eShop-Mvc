﻿using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Core.Entities
{
    public class SystemConfig : BaseEntity<int>, ISwitchable
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(25)]
        public string Type { get; set; }

        public string Value { get; set; }

        public Status Status { get; set; }
    }
}