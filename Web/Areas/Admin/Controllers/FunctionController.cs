using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Services.Command.FunctionCommand;
using eShop_Mvc.Core.Services.Query.FunctionQuery;
using eShop_Mvc.Models.System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<FunctionController> _logger;

        public FunctionController(IMapper mapper, IMediator mediator, ILogger<FunctionController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
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
                Keyword = filter
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
            var model = _mapper.Map<IReadOnlyList<Function>, IReadOnlyList<FunctionViewModel>>(await _mediator.Send(new GetAllFunctionWithFilterQuery()));
            var rootFunction = model.Where(f => f.ParentId == null);
            var items = new List<FunctionViewModel>();
            foreach (var function in rootFunction)
            {
                items.Add(function);
                GetByParentId(model, function, items);
            }

            return new OkObjectResult(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var model = _mapper.Map<Function, FunctionViewModel>(await _mediator.Send(new GetFunctionByIdQuery()
            {
                FunctionId = id
            }));
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

            var function = _mapper.Map<FunctionViewModel, Function>(functionViewModel);

            if (string.IsNullOrEmpty(functionViewModel.Id))
            {
                function.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _mediator.Send(new CreateFunctionCommand()
                {
                    Function = function
                });
                _logger.LogInformation("New function is created successfully !");
            }
            else
            {
                function.ModifiedBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _mediator.Send(new UpdateFunctionCommand()
                {
                    Function = function
                });
                _logger.LogInformation($"function {functionViewModel.Id} had been modified.");
            }
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

            await _mediator.Send(new UpdateFunctionParentIdCommand()
            {
                SourceId = sourceId,
                TargetId = targetId,
                SubFunctionData = items
            });

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

            await _mediator.Send(new UpdateFunctionOrderCommand()
            {
                SourceId = sourceId,
                TargetId = targetId
            });
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new BadRequestResult();
            }

            await _mediator.Send(new DeleteFunctionCommand()
            {
                FunctionId = id
            });
            _logger.LogInformation($"Function id: {id} had been deleted!");
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