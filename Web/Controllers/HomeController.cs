using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Services.Query.CategoryQuery;
using eShop_Mvc.Core.Services.Query.ProductQuery;
using eShop_Mvc.Models;
using eShop_Mvc.Models.ProductViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace eShop_Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public HomeController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["BodyClass"] = "cms-index-index cms-home-page";
            //var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var homeCategories = await _mediator.Send(new GetHomeCategoryQuery()
            {
                Top = 5
            });
            var latestProduct = await _mediator.Send(new GetLatestProductQuery()
            {
                Top = 5
            });
            var hotProducts = await _mediator.Send(new GetHotProductQuery()
            {
                Top = 5
            });
            var homeViewModel = new HomeViewModel
            {
                HomeCategories = _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(homeCategories),
                HotProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(hotProducts),
                LatestProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(latestProduct)
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