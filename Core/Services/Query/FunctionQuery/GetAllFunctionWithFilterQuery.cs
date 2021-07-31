using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.FunctionQuery
{
    public class GetAllFunctionWithFilterQuery : IRequest<IReadOnlyList<Function>>
    {
        public string Filter { get; set; }
    }
}