using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.TagQuery
{
    public class GetTagByProductIdQuery : IRequest<IReadOnlyList<Tag>>
    {
        public int ProductId { get; set; }
    }
}