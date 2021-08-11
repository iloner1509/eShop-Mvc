using MediatR;
using System.Collections.Generic;

namespace eShop_Mvc.Core.Services.Command.CategoryCommand
{
    public class UpdateCategoryParentIdCommand : IRequest
    {
        public int SourceId { get; set; }
        public int TargetId { get; set; }
        public Dictionary<int, int> SubCategoryData { get; set; }
    }
}