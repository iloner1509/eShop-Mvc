using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Services.Query.CategoryQuery;
using eShop_Mvc.Core.Services.Query.ProductQuery;
using eShop_Mvc.Core.Services.Query.TagQuery;
using eShop_Mvc.Core.Specifications.ProductSpecification;
using eShop_Mvc.Models.Common;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShop_Mvc.SharedKernel.Enums;
using Microsoft.Extensions.Configuration;

namespace eShop_Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public ProductController(IMapper mapper, IMediator mediator, IConfiguration configuration)
        {
            _mapper = mapper;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _mediator.Send(new GetAllCategoryQuery());
            var model = _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(categories);
            return View(model);
        }

        public async Task<IActionResult> Catalog(int id, string searchKeyword, SortTypes sortTypes, int? pageSize, int pageIndex = 1)
        {
            ViewData["BodyClass"] = "shop_list_page";
            pageSize ??= _configuration.GetValue<int>("PageSize");
            var pagingParams = new ProductPagingParams()
            {
                CategoryId = id,
                PageIndex = pageIndex,
                PageSize = pageSize.Value,
                SearchKeyword = searchKeyword,
                Sort = sortTypes
            };
            var result = await _mediator.Send(new GetAllProductPagingQuery()
            {
                PagingParams = pagingParams
            });
            var listCategory = await _mediator.Send(new GetAllCategoryQuery());
            var currentCategory = _mapper.Map<ProductCategory, ProductCategoryViewModel>(await _mediator.Send(new GetCategoryByIdQuery()
            {
                CategoryId = id
            }));
            var catalog = new CatalogViewModel
            {
                PageSize = pagingParams.PageSize,
                SortType = pagingParams.Sort.ToString(),
                Data = new PagedResult<ProductViewModel>()
                {
                    CurrentPage = result.CurrentPage,
                    RowCount = result.RowCount,
                    PageSize = result.PageSize,
                    Results = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(result.Results)
                },
                Category = currentCategory,
                ListCategory = listCategory.Select(x => x.Name).ToList(),
            };

            return View(catalog);
        }

        public async Task<IActionResult> Search(ProductPagingParams pagingParams)
        {
            ViewData["BodyClass"] = "shop_list_page";
            var result = await _mediator.Send(new GetAllProductPagingQuery()
            {
                PagingParams = pagingParams
            });
            var catalog = new SearchViewModel()
            {
                PageSize = pagingParams.PageSize,
                SortType = pagingParams.Sort.ToString(),
                Data = new PagedResult<ProductViewModel>()
                {
                    CurrentPage = result.CurrentPage,
                    RowCount = result.RowCount,
                    PageSize = result.PageSize,
                    Results = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(result.Results)
                },
                Keyword = pagingParams.SearchKeyword
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