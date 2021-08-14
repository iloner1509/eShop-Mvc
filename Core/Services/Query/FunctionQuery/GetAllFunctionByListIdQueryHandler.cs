using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.FunctionSpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.FunctionQuery
{
    public class GetAllFunctionByListIdQueryHandler : IRequestHandler<GetAllFunctionByListIdQuery, IReadOnlyList<Function>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllFunctionByListIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IReadOnlyList<Function>> Handle(GetAllFunctionByListIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new FunctionByListFunctionIdSpecification(request.ListFunctionId);
            return _unitOfWork.Repository<Function, string>().FindAllAsync(cancellationToken, specification);
        }
    }
}