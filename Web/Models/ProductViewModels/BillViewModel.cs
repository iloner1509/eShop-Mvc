using eShop_Mvc.Models.AccountViewModels;
using eShop_Mvc.SharedKernel.Enums;
using System;
using System.Collections.Generic;

namespace eShop_Mvc.Models.ProductViewModels
{
    public class BillViewModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerMobile { get; set; }

        public string CustomerMessage { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public BillStatus BillStatus { get; set; }

        public Guid? CustomerId { get; set; }
        public AppUserViewModel User { get; set; }
        public ICollection<BillDetailViewModel> BillDetails { get; set; }

        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}