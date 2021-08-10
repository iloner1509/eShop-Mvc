using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.CategoryCommand
{
    public class CreateCategoryCommand : IRequest
    {
        public ProductCategory Category { get; set; }
    }
}