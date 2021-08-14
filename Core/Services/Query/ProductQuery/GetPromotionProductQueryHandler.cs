using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.ProductSpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetPromotionProductQueryHandler : IRequestHandler<GetPromotionProductQuery, IReadOnlyList<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPromotionProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> Handle(GetPromotionProductQuery request, CancellationToken cancellationToken)
        {
            var specification = new PromotionProductSpecification(request.Top);
            return await _unitOfWork.Repository<Product, int>().FindAllAsync(cancellationToken, specification);
        }
    }
}