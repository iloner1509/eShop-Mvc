using eShop_Mvc.Core.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Query.FunctionQuery
{
    public class GetFunctionByIdQueryHandler : IRequestHandler<GetFunctionByIdQuery, Function>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFunctionByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Function> Handle(GetFunctionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Function, string>().FindByIdAsync(request.FunctionId);
        }
    }
}