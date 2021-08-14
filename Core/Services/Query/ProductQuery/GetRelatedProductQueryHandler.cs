using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.ProductSpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetRelatedProductQueryHandler : IRequestHandler<GetRelatedProductQuery, IReadOnlyList<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRelatedProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> Handle(GetRelatedProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product, int>().FindByIdAsync(request.ProductId);
            var specification = new RelatedProductSpecification(request.ProductId, product.CategoryId, request.Top);
            return await _unitOfWork.Repository<Product, int>().FindAllAsync(cancellationToken, specification);
        }
    }
}