using eShop_Mvc.Core.Services.Query.PermissionQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eShop_Mvc.Authorization
{
    public class ResourceBasedAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, string>
    {
        private readonly IMediator _mediator;

        public ResourceBasedAuthorizationHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            string resource)
        {
            var roles = ((ClaimsIdentity)context.User.Identity).Claims.FirstOrDefault(x => x.Type == "Roles");
            if (roles != null)
            {
                var listRole = roles.Value.Split(";");
                var hasPermission = await _mediator.Send(new CheckPermissionQuery()
                {
                    FunctionId = resource,
                    Action = requirement.Name,
                    Roles = listRole
                });
                if (hasPermission || listRole.Contains("Admin"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}