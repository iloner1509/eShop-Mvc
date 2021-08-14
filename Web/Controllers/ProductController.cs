using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Models.Common;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShop_Mvc.Core.Services.Query.CategoryQuery;
using eShop_Mvc.Core.Services.Query.ProductQuery;
using eShop_Mvc.Core.Services.Query.TagQuery;
using MediatR;

namespace eShop_Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ProductController(IMapper mapper, IConfiguration configuration, IMediator mediator)
        {
            _mapper = mapper;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _mediator.Send(new GetAllCategoryQuery());
            var model = _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(categories);
            return View(model);
        }

        public async Task<IActionResult> Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = "shop_list_page";
            pageSize ??= _configuration.GetValue<int>("PageSize");
            var result = await _productService.GetAllPagingAsync(id, string.Empty, page, pageSize.Value);
            var listCategory = await _productCategoryService.GetAllAsync();
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
                Category = _mapper.Map<ProductCategory, ProductCategoryViewModel>(await _productCategoryService.GetByIdAsync(id)),
                ListCategory = listCategory.Select(x => x.Name).ToList(),
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
            var relatedProduct = await _mediator.Send(new GetRelatedProductQuery()
            {
                ProductId = id,
                Top = 10
            });
            var product = await _mediator.Send(new GetProductByIdQuery()
            {
                ProductId = id
            });
            var category = await _mediator.Send(new GetCategoryByIdQuery()
            {
                CategoryId = product.CategoryId
            });
            var tags = await _mediator.Send(new GetTagByProductIdQuery()
            {
                ProductId = id
            });
            var model = new DetailViewModel
            {
                Product = _mapper.Map<Product, ProductViewModel>(product),
                Category = _mapper.Map<ProductCategory, ProductCategoryViewModel>(category),
                RelatedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(relatedProduct),
                Available = product.Quantity > 0,
                Tags = _mapper.Map<IReadOnlyList<Tag>, IReadOnlyList<TagViewModel>>(tags)
            };

            return View(model);
        }
    }
}