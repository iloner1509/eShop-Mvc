using MediatR;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class DeleteFunctionCommand : IRequest
    {
        public string FunctionId { get; set; }
    }
}