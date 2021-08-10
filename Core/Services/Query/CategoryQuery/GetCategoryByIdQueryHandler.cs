using eShop_Mvc.Core.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Query.CategoryQuery
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ProductCategory>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductCategory> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<ProductCategory, int>().FindByIdAsync(request.CategoryId);
        }
    }
}