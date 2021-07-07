using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services.Query
{
    public class GetAllFunctionWithFilterQueryHandler : IRequestHandler<GetAllFunctionWithFilterQuery, IReadOnlyList<Function>>
    {
        private readonly IRepository<Function, string> _functionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllFunctionWithFilterQueryHandler(IRepository<Function, string> functionRepository, IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Function>> Handle(GetAllFunctionWithFilterQuery request, CancellationToken cancellationToken)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(x => x.Name.Contains(request.Filter));
            }

            return await query.OrderBy(x => x.ParentId).ToListAsync(cancellationToken);
        }
    }
}