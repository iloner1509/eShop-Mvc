using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Core.Services.Query.FunctionQuery;
using eShop_Mvc.Extensions;
using eShop_Mvc.Models.System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SideBarViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains("Admin"))
            {
                functions = _mapper.Map<IReadOnlyList<Function>, List<FunctionViewModel>>(await _mediator.Send(new GetAllFunctionWithFilterQuery()));
            }
            else
            {
                // Todo: get by permission
                functions = _mapper.Map<IReadOnlyList<Function>, List<FunctionViewModel>>(await _mediator.Send(new GetAllFunctionWithFilterQuery()));
            }
            return View(functions);
        }
    }
}