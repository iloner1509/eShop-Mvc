using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.UserQuery
{
    public class GetUserByIdQuery : IRequest<AppUser>
    {
        public string UserId { get; set; }
    }
}