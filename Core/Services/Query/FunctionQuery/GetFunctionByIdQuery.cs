using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.FunctionQuery
{
    public class GetFunctionByIdQuery : IRequest<Function>
    {
        public string FunctionId { get; set; }
    }
}