using System;
using System.Collections.Generic;
using System.Linq;
using eShop_Mvc.Models.Common;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Models
{
    public class CheckoutViewModel : ProductViewModel
    {
        public List<CartViewModel> Cart { get; set; }

        public List<EnumModel> PaymentMethods
        {
            get
            {
                return ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod))).Select(e => new EnumModel()
                {
                    Value = (int)e,
                    Name = e.ToString()
                }).ToList();
            }
        }
    }
}