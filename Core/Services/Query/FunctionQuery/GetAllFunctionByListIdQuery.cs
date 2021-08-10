using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.FunctionQuery
{
    public class GetAllFunctionByListIdQuery : IRequest<IReadOnlyList<Function>>
    {
        public IEnumerable<string> ListFunctionId { get; set; }
    }
}