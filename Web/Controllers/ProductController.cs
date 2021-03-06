﻿using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Models.Common;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop_Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IBillService _billService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IBillService billService, IMapper mapper, IConfiguration configuration)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _billService = billService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var categories = _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(await _productCategoryService.GetAllAsync());
            return View(categories);
        }

        public async Task<IActionResult> Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = "shop_list_page";
            pageSize ??= _configuration.GetValue<int>("PageSize");
            var result = await _productService.GetAllPagingAsync(id, string.Empty, page, pageSize.Value);
            var catalog = new CatalogViewModel
            {
                PageSize = pageSize,
                SortType = sortBy,
                Data = new PagedResult<ProductViewModel>()
                {
                    CurrentPage = result.CurrentPage,
                    RowCount = result.RowCount,
                    PageSize = result.PageSize,
                    Results = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(result.Results)
                },
                Category = _mapper.Map<ProductCategory, ProductCategoryViewModel>(await _productCategoryService.GetByIdAsync(id))
            };

            return View(catalog);
        }

        public async Task<IActionResult> Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = "shop_list_page";
            pageSize ??= _configuration.GetValue<int>("PageSize");
            var result = await _productService.GetAllPagingAsync(null, keyword, page, pageSize.Value);
            var catalog = new SearchViewModel()
            {
                PageSize = pageSize,
                SortType = sortBy,
                Data = new PagedResult<ProductViewModel>()
                {
                    CurrentPage = result.CurrentPage,
                    RowCount = result.RowCount,
                    PageSize = result.PageSize,
                    Results = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(result.Results)
                },
                Keyword = keyword
            };

            return View(catalog);
        }

        public async Task<IActionResult> Detail(int id)
        {
            ViewData["BodyClass"] = "product-page";
            var model = new DetailViewModel();
            model.Product = _mapper.Map<Product, ProductViewModel>(await _productService.GetByIdAsync(id));
            model.Category = _mapper.Map<ProductCategory, ProductCategoryViewModel>(await _productCategoryService.GetByIdAsync(model.Product.CategoryId));
            model.RelatedProducts =
                _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(
                    await _productService.GetRelatedProductsAsync(id, 9));

            model.Tags =
                _mapper.Map<IReadOnlyList<Tag>, IReadOnlyList<TagViewModel>>(
                    await _productService.GetProductTagsAsync(id));
            model.Available = model.Product.Quantity > 0;
            return View(model);
        }
    }
}