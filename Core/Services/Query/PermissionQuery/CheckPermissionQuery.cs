using MediatR;

namespace eShop_Mvc.Core.Services.Query.PermissionQuery
{
    public class CheckPermissionQuery : IRequest<bool>
    {
        public string FunctionId { get; set; }
        public string Action { get; set; }

        public string[] Roles { get; set; }
    }
}