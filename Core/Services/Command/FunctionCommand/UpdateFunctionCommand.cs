using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class UpdateFunctionCommand : IRequest
    {
        public Function Function { get; set; }
    }
}