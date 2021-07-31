using System.Collections.Generic;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.TagQuery
{
    public class GetAllProductTagNameQuery : IRequest<List<string>>
    {
    }
}