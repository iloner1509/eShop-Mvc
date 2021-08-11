using eShop_Mvc.Core.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Product, int>().FindByIdAsync(request.ProductId);
        }
    }
}