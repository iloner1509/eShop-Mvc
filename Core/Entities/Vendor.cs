using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Entities
{
    public class Vendor : BaseEntity<int>, IAuditable, ISwitchable
    {
        public Vendor(string vendorName, string address, string phone, string email, int vendorTypeId, string contactPerson, string createdBy)
        {
            VendorName = vendorName;
            Address = address;
            Phone = phone;
            Email = email;
            VendorTypeId = vendorTypeId;
            ContactPerson = contactPerson;
            CreatedBy = createdBy;
        }

        [StringLength(100)]
        [Required]
        public string VendorName { get; set; }

        public string Address { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int VendorTypeId { get; set; }
        public VendorType VendorType { get; set; }

        [StringLength(25)]
        public string ContactPerson { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}