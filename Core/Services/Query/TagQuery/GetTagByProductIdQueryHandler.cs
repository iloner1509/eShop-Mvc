using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.TagSpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.TagQuery
{
    public class GetTagByProductIdQueryHandler : IRequestHandler<GetTagByProductIdQuery, IReadOnlyList<Tag>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTagByProductIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Tag>> Handle(GetTagByProductIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductTagByProductIdSpecification(request.ProductId);
            return await _unitOfWork.Repository<ProductTag, int>().ApplySpecification(specification).Select(x => x.Tag).ToListAsync(cancellationToken);
        }
    }
}