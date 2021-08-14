using System.Security.Claims;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IUserService
    {
        ClaimsPrincipal GetUser();
    }
}