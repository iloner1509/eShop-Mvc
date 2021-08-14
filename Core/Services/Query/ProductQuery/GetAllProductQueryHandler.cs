using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.ProductSpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IReadOnlyList<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductIncludeProductCategorySpecification();
            return await _unitOfWork.Repository<Product, int>().FindAllAsync(cancellationToken, specification);
        }
    }
}