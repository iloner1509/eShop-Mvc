using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace eShop_Mvc.Core.Services.Command.UserCommand
{
    public class AddUserToRolesCommand : IRequest<IdentityResult>
    {
        public AppUser User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}