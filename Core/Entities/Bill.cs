using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop_Mvc.Core.Entities
{
    public class Bill : BaseEntity<int>, ISwitchable, IDateTracking
    {
        public Bill()
        {
        }

        public Bill(string customerName, string customerAddress, string customerMobile,
            string customerMessage, PaymentMethod paymentMethod, BillStatus billStatus, Guid? customerId, Status status)
        {
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            PaymentMethod = paymentMethod;
            BillStatus = billStatus;
            CustomerId = customerId;

            Status = status;
        }

        public Bill(int id, string customerName, string customerAddress, string customerMobile,
            string customerMessage, PaymentMethod paymentMethod, BillStatus billStatus, Guid? customerId, Status status)
        {
            Id = id;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            PaymentMethod = paymentMethod;
            BillStatus = billStatus;
            CustomerId = customerId;

            Status = status;
        }

        [StringLength(150)]
        [Required]
        public string CustomerName { get; set; }

        [StringLength(250)]
        [Required]
        public string CustomerAddress { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        [Required]
        public string CustomerMobile { get; set; }

        public string CustomerMessage { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public BillStatus BillStatus { get; set; }

        public Guid? CustomerId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }

        [Required]
        [DefaultValue(Status.Active)]
        public Status Status { get; set; } = Status.Active;

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}