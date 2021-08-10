using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.FunctionQuery
{
    public class GetAllFunctionWithFilterQueryHandler : IRequestHandler<GetAllFunctionWithFilterQuery, IReadOnlyList<Function>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllFunctionWithFilterQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Function>> Handle(GetAllFunctionWithFilterQuery request, CancellationToken cancellationToken)
        {
            var specification = new FunctionWithFilterSpecification(request.Keyword);

            return await _unitOfWork.Repository<Function, string>().FindAllAsync(cancellationToken, specification);
        }
    }
}