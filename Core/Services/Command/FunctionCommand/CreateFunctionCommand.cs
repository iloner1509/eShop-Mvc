using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class CreateFunctionCommand : IRequest
    {
        public Function Function { get; set; }
    }
}