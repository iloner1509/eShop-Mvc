using System;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.RoleQuery
{
    public class GetRoleByIdQuery : IRequest<AppRole>
    {
        public Guid RoleId { get; set; }
    }
}