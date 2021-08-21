using System;
using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.PermissionCommand
{
    public class SavePermissionCommand : IRequest
    {
        public IList<Permission> ListPermission { get; set; }
        public Guid RoleId { get; set; }
    }
}