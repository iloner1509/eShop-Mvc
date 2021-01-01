using eShop_Mvc.Core.Enums;

namespace eShop_Mvc.Core.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}