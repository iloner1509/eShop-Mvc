using MediatR;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Services.Query.UserQuery
{
    public class LoginQuery : IRequest<SignInResult>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}