using eShop_Mvc.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Services.Command.UserCommand
{
    public class CreateUserCommand : IRequest<IdentityResult>
    {
        public AppUser AppUser { get; set; }
        public string Password { get; set; }
    }
}