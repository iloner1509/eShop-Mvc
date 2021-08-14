using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.ProductSpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetLatestProductQueryHandler : IRequestHandler<GetLatestProductQuery, IReadOnlyList<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLatestProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> Handle(GetLatestProductQuery request, CancellationToken cancellationToken)
        {
            var specification = new LatestProductSpecification(request.Top);
            return await _unitOfWork.Repository<Product, int>().FindAllAsync(cancellationToken, specification);
        }
    }
}