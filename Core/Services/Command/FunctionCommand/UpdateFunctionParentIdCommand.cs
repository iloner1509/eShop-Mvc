using System.Collections.Generic;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.FunctionCommand
{
    public class UpdateFunctionParentIdCommand : IRequest
    {
        public string SourceId { get; set; }
        public string TargetId { get; set; }
        public Dictionary<string, int> SubFunctionData { get; set; }
    }
}