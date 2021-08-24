using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.UserQuery
{
    public class GetUserByNameQuery : IRequest<AppUser>
    {
        public string Username { get; set; }
    }
}