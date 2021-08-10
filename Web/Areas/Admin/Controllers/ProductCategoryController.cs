﻿using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShop_Mvc.Core.Services.Query.CategoryQuery;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCategoryController> _logger;
        private readonly IMediator _mediator;

        public ProductCategoryController(IProductCategoryService productCategoryService, IMapper mapper, ILogger<ProductCategoryController> logger, IMediator mediator)
        {
            _productCategoryService = productCategoryService;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Get data API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _mediator.Send(new GetAllCategoryQuery());
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _mediator.Send(new GetCategoryByIdQuery()
            {
                CategoryId = id
            });
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (sourceId == targetId)
            {
                return new BadRequestResult();
            }

            await _productCategoryService.UpdateParentIdAsync(sourceId, targetId, items);
            _productCategoryService.Save();
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> ReOrder(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (sourceId == targetId)
            {
                return new BadRequestResult();
            }

            await _productCategoryService.ReOrderAsync(sourceId, targetId);
            _productCategoryService.Save();
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(ProductCategoryViewModel productCategoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            productCategoryViewModel.SeoAlias = TextHelper.ToUnsignString(productCategoryViewModel.Name);
            if (productCategoryViewModel.Id == 0)
            {
                await _productCategoryService.AddAsync(_mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryViewModel));
            }
            else
            {
                await _productCategoryService.UpdateAsync(_mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryViewModel));
            }
            _productCategoryService.Save();
            return new OkObjectResult(productCategoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }

            await _productCategoryService.DeleteAsync(id);
            _productCategoryService.Save();
            return new OkObjectResult(id);
        }

        #endregion Get data API
    }
}