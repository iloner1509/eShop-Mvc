using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
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

        #endregion Ajax api
    }
}