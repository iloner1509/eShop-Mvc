using MediatR;

namespace eShop_Mvc.Core.Services.Command.CategoryCommand
{
    public class DeleteCategoryCommand : IRequest
    {
        public int CategoryId { get; set; }
    }
}