using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.CategoryQuery
{
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, IReadOnlyList<ProductCategory>>
    {
        private readonly IRepository<ProductCategory, int> _categoryRepository;

        public GetAllCategoryQueryHandler(IRepository<ProductCategory, int> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IReadOnlyList<ProductCategory>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductCategoryWithFilterSpecification(keyword: request.Keyword);
            return await _categoryRepository.ApplySpecification(specification).ToListAsync(cancellationToken);
        }
    }
}