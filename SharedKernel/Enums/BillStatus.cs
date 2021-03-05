using System.ComponentModel;

namespace eShop_Mvc.SharedKernel.Enums
{
    public enum BillStatus
    {
        [Description("Mới")]
        New,

        [Description("Đang xử lý")]
        Inprogress,

        [Description("Hoàn trả")]
        Returned,

        [Description("Bị Hủy")]
        Cancelled,

        [Description("Thành công")]
        Completed
    }
}