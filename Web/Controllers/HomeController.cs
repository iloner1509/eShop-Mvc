using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Models;
using eShop_Mvc.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MediatR;

namespace eShop_Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IProductCategoryService productCategoryService, IMapper mapper, IMediator mediator)
        {
            _logger = logger;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["BodyClass"] = "cms-index-index cms-home-page";
            //var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var homeViewModel = new HomeViewModel
            {
                HomeCategories =
                _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(
                    await _productCategoryService.GetHomeCategoriesAsync(5)),
                HotProducts =
                _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(
                    await _productService.GetHotProductsAsync(5)),
                TopSellProducts =
                _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(
                    await _productService.GetLatestAsync(5))
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}