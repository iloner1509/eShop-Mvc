using MediatR;

namespace eShop_Mvc.Core.Services.Command.CategoryCommand
{
    public class UpdateCategoryOrderCommand : IRequest
    {
        public int SourceId { get; set; }
        public int TargetId { get; set; }
    }
}