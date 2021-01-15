using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.SharedKernel.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}