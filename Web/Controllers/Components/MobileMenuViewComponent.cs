using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Core.Services.Query.CategoryQuery;
using eShop_Mvc.Models.ProductViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Controllers.Components
{
    public class MobileMenuViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MobileMenuViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryList = await _mediator.Send(new GetAllCategoryQuery());
            return View(_mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(categoryList));
        }
    }
}