using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.CategoryQuery
{
    public class GetAllByParentIdQueryHandler : IRequestHandler<GetAllByParentIdQuery, IReadOnlyList<ProductCategory>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllByParentIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<ProductCategory>> Handle(GetAllByParentIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new CategoryByParentIdSpecification(request.ParentId);
            return await _unitOfWork.Repository<ProductCategory, int>().FindAllAsync(cancellationToken, specification);
        }
    }
}