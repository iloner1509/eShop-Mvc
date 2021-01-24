using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Extensions;
using eShop_Mvc.Models.System;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly IFunctionService _functionService;
        private readonly IMapper _mapper;

        public SideBarViewComponent(IFunctionService functionService, IMapper mapper)
        {
            _functionService = functionService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains("Admin"))
            {
                functions = _mapper.Map<List<FunctionViewModel>>(await _functionService.GetAllAsync(String.Empty));
            }
            else
            {
                // To do: get by permission
                functions = _mapper.Map<List<FunctionViewModel>>(await _functionService.GetAllAsync(String.Empty));
            }
            return View(functions);
        }
    }
}