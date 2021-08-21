using System;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.RoleCommand
{
    public class DeleteRoleCommand : IRequest<bool>
    {
        public Guid RoleId { get; set; }
    }
}