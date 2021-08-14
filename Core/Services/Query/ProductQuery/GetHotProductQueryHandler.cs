using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.ProductSpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class GetHotProductQueryHandler : IRequestHandler<GetHotProductQuery, IReadOnlyList<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetHotProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> Handle(GetHotProductQuery request, CancellationToken cancellationToken)
        {
            var listBillDetail = await _unitOfWork.Repository<BillDetail, int>().ListAllAsync(cancellationToken);
            var listHotProductData = (from bd in listBillDetail
                                      group bd by new { bd.ProductId } into t
                                      select new
                                      {
                                          t.Key.ProductId,
                                          Quantity = t.Sum(bd => bd.Quantity)
                                      }).OrderByDescending(x => x.Quantity).Select(x => x.ProductId).Take(request.Top);

            // get all Product by list productId
            var specification = new ProductByListProductIdSpecification(listHotProductData);
            return await _unitOfWork.Repository<Product, int>().FindAllAsync(cancellationToken, specification);
        }
    }
}