using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services.Query.TagQuery
{
    public class GetAllProductTagNameQueryHandler : IRequestHandler<GetAllProductTagNameQuery, List<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductTagNameQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<string>> Handle(GetAllProductTagNameQuery request, CancellationToken cancellationToken)
        {
            var specification = new TagWithTypeSpecification("Product");
            return await _unitOfWork.Repository<Tag, string>().ApplySpecification(specification).Select(t => t.Name).ToListAsync(cancellationToken);
        }
    }
}