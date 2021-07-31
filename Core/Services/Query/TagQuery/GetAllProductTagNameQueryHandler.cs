using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services.Query.TagQuery
{
    public class GetAllProductTagNameQueryHandler : IRequestHandler<GetAllProductTagNameQuery, List<string>>
    {
        private readonly IRepository<Tag, string> _tagRepository;

        public GetAllProductTagNameQueryHandler(IRepository<Tag, string> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<List<string>> Handle(GetAllProductTagNameQuery request, CancellationToken cancellationToken)
        {
            return await _tagRepository.FindAll(x => x.Type == "Product").Select(x => x.Name)
                .ToListAsync(cancellationToken);
        }
    }
}