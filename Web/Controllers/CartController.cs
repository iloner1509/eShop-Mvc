using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Extensions;
using eShop_Mvc.Models;
using eShop_Mvc.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop_Mvc.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        public CartController(IProductService productService, IMapper mapper)
        {
            _productService = productService;

            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        #region Ajax request

        [HttpGet]
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>("CartSession") ?? new List<CartViewModel>();
            return new OkObjectResult(session);
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("CartSession");
            return new OkObjectResult("Ok");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = _mapper.Map<Product, ProductViewModel>(await _productService.GetByIdAsync(productId));

            // get session
            var session = HttpContext.Session.Get<List<CartViewModel>>("CartSession");
            if (session != null)
            {
                bool hasChanged = false;

                // check exist item with product id
                if (session.Any(x => x.Product.Id == productId))
                {
                    foreach (var item in session)
                    {
                        // update quantity if productId is existed
                        if (item.Product.Id == productId)
                        {
                            item.Quantity += quantity;
                            item.Price = product.PromotionPrice ?? product.Price;
                            hasChanged = true;
                        }
                    }
                }
                else
                {
                    session.Add(new CartViewModel()
                    {
                        Product = product,
                        Quantity = quantity,
                        Price = product.PromotionPrice ?? product.Price
                    });
                    hasChanged = true;
                }

                // update cart
                if (hasChanged)
                {
                    HttpContext.Session.Set("CartSession", session);
                }
            }
            else
            {
                var cart = new List<CartViewModel>
                {
                    new CartViewModel()
                    {
                        Product = product,
                        Quantity = quantity,
                        Price = product.PromotionPrice ?? product.Price
                    }
                };
                HttpContext.Session.Set("CartSession", cart);
            }
            return new OkObjectResult(productId);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>("CartSession");
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }

                if (hasChanged)
                {
                    HttpContext.Session.Set("CartSession", session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        public async Task<IActionResult> UpdateCart(int productId, int quantity)
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>("CartSession");
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = _mapper.Map<Product, ProductViewModel>(await _productService.GetByIdAsync(productId));
                        item.Product = product;
                        item.Quantity = quantity;
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }

                if (hasChanged)
                {
                    HttpContext.Session.Set("CartSession", session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        #endregion Ajax request
    }
}