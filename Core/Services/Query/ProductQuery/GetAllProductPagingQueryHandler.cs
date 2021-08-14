using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Specifications.ProductSpecification;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetAllProductPagingQueryHandler : IRequestHandler<GetAllProductPagingQuery, PagedResult<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductPagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Product>> Handle(GetAllProductPagingQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductWithFilterSpecification(request.PagingParams);
            var totalRow = await _unitOfWork.Repository<Product, int>().CountAsync(cancellationToken, specification);
            var data = await _unitOfWork.Repository<Product, int>().FindAllAsync(cancellationToken, specification);
            return new PagedResult<Product>()
            {
                CurrentPage = request.PagingParams.PageIndex,
                RowCount = totalRow,
                Results = data,
                PageSize = request.PagingParams.PageSize,
            };
        }
    }
}