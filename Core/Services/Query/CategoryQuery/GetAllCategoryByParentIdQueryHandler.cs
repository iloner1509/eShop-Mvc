﻿using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.CategorySpecification;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.CategoryQuery
{
    public class GetAllCategoryByParentIdQueryHandler : IRequestHandler<GetAllCategoryByParentIdQuery, IReadOnlyList<ProductCategory>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCategoryByParentIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<ProductCategory>> Handle(GetAllCategoryByParentIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new CategoryByParentIdSpecification(request.ParentId);
            return await _unitOfWork.Repository<ProductCategory, int>().FindAllAsync(cancellationToken, specification);
        }
    }
}