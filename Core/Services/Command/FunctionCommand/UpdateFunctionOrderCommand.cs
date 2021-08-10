using MediatR;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class UpdateFunctionOrderCommand : IRequest
    {
        public string SourceId { get; set; }
        public string TargetId { get; set; }
    }
}