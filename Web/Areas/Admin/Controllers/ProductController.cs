using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Helpers;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Ajax api

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(await _productService.GetAllAsync());
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var model = _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(await _productCategoryService.GetAllAsync());
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = await _productService.GetAllPagingAsync(categoryId, keyword, page, pageSize);

            return new OkObjectResult(new PagedResult<ProductViewModel>()
            {
                CurrentPage = model.CurrentPage,
                PageSize = model.PageSize,
                RowCount = model.RowCount,
                Results = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(model.Results)
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = _mapper.Map<Product, ProductViewModel>(await _productService.GetByIdAsync(id));
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productViewModel.SeoAlias = TextHelper.ToUnsignString(productViewModel.Name);
                if (productViewModel.Id == 0)
                {
                    await _productService.AddAsync(_mapper.Map<ProductViewModel, Product>(productViewModel));
                }
                else
                {
                    await _productService.UpdateAsync(_mapper.Map<ProductViewModel, Product>(productViewModel));
                }
                _productService.Save();
                return new OkObjectResult(productViewModel);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                await _productService.DeleteAsync(id);
                _productService.Save();
                return new OkObjectResult(id);
            }
        }

        #endregion Ajax api
    }
}