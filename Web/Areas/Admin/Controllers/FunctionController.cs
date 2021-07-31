using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Core.Services.Query.FunctionQuery;
using eShop_Mvc.Models.System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly IFunctionService _functionService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public FunctionController(IFunctionService functionService, IMapper mapper, IMediator mediator)
        {
            _functionService = functionService;
            _mapper = mapper;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFilter(string filter)
        {
            //var model = await _functionService.GetAllAsync(filter);
            var model = await _mediator.Send(new GetAllFunctionWithFilterQuery()
            {
                Filter = filter
            });
            return new OkObjectResult(_mapper.Map<IReadOnlyList<Function>, IReadOnlyList<FunctionViewModel>>(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var model = _mapper.Map<IReadOnlyList<Function>, IReadOnlyList<FunctionViewModel>>(await _mediator.Send(new GetAllFunctionWithFilterQuery()
            //{
            //    Filter = string.Empty
            //}));
            var model = _mapper.Map<IReadOnlyList<Function>, IReadOnlyList<FunctionViewModel>>(await _functionService.GetAllAsync(string.Empty));
            var rootFunction = model.Where(f => f.ParentId == null);
            var items = new List<FunctionViewModel>();
            foreach (var function in rootFunction)
            {
                items.Add(function);
                GetByParentId(model.ToList(), function, items);
            }

            return new OkObjectResult(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var model = _mapper.Map<Function, FunctionViewModel>(await _functionService.GetByIdAsync(id));
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(FunctionViewModel functionViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (string.IsNullOrEmpty(functionViewModel.Id))
            {
                await _functionService.AddAsync(_mapper.Map<FunctionViewModel, Function>(functionViewModel));
            }
            else
            {
                await _functionService.UpdateAsync(_mapper.Map<FunctionViewModel, Function>(functionViewModel));
            }
            _functionService.Save();
            return new OkObjectResult(functionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (sourceId == targetId)
            {
                return new BadRequestResult();
            }

            await _functionService.UpdateParentIdAsync(sourceId, targetId, items);
            _functionService.Save();
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> ReOrder(string sourceId, string targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (sourceId == targetId)
            {
                return new BadRequestResult();
            }

            await _functionService.ReOrderAsync(sourceId, targetId);
            _functionService.Save();
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new BadRequestResult();
            }

            await _functionService.DeleteAsync(id);
            _functionService.Save();
            return new OkObjectResult(id);
        }

        #region private function

        private void GetByParentId(IEnumerable<FunctionViewModel> allFunctions, FunctionViewModel parent, IList<FunctionViewModel> items)
        {
            var functionEntities = allFunctions as FunctionViewModel[] ?? allFunctions.ToArray();
            var subFunction = functionEntities.Where(f => f.ParentId == parent.Id);
            foreach (var function in subFunction)
            {
                items.Add(function);
                GetByParentId(functionEntities, function, items);
            }
        }

        #endregion private function
    }
}