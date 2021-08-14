using eShop_Mvc.Core.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace eShop_Mvc.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal GetUser()
        {
            return _httpContextAccessor?.HttpContext?.User;
        }
    }
}