using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Services.Command.UserCommand
{
    public class UpdateUserCommand : IRequest<IdentityResult>
    {
        public AppUser AppUser { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}