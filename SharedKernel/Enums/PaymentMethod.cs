using System.ComponentModel;

namespace eShop_Mvc.SharedKernel.Enums
{
    public enum PaymentMethod
    {
        [Description("Thanh toán khi nhận hàng")]
        CashOnDelivery,

        [Description("Thanh toán trực tuyến")]
        OnlineBanking,

        [Description("Thanh toán qua cổng thanh toán")]
        PaymentGateway,

        [Description("Thanh toán qua thẻ Visa")]
        Visa,

        [Description("Thanh toán qua MasterCard")]
        MasterCard,

        [Description("Thanh toán qua PayPal")]
        Paypal,

        [Description("Thanh toán qua ATM")]
        ATM
    }
}