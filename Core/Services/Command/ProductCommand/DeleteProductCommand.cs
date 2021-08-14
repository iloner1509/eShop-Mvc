using MediatR;

namespace eShop_Mvc.Core.Services.Command.ProductCommand
{
    public class DeleteProductCommand : IRequest
    {
        public int ProductId { get; set; }
    }
}