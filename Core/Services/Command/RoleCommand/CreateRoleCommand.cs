using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.RoleCommand
{
    public class CreateRoleCommand : IRequest<bool>
    {
        public Announcement Announcement { get; set; }
        public AppRole Role { get; set; }
    }
}