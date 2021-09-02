using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Services.Command.BillCommand;
using eShop_Mvc.Core.Services.Query.ProductQuery;
using eShop_Mvc.Extensions;
using eShop_Mvc.Models;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop_Mvc.Controllers
{
    public class CartController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CartController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var model = new CheckoutViewModel();
            var session = HttpContext.Session.Get<List<CartViewModel>>("CartSession");
            model.Cart = session;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Cart != null && model.Cart.Any())
                {
                    var details = new List<BillDetailViewModel>();
                    foreach (var item in model.Cart)
                    {
                        details.Add(new BillDetailViewModel()
                        {
                            //Product = item.Product,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id
                        });
                    }
                    var bill = new BillViewModel()
                    {
                        CustomerMobile = model.CustomerMobile,
                        CustomerName = model.CustomerName,
                        CustomerAddress = model.CustomerAddress,
                        CustomerMessage = model.CustomerMessage,
                        BillDetails = details,
                        BillStatus = BillStatus.New
                    };
                    if (User.Identity.IsAuthenticated)
                    {
                        bill.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
                    }

                    try
                    {
                        await _mediator.Send(new CreateBillCommand()
                        {
                            Bill = _mapper.Map<BillViewModel, Bill>(bill)
                        });
                        ViewData["Success"] = true;
                    }
                    catch (Exception e)
                    {
                        ViewData["Success"] = false;
                        ModelState.AddModelError("CheckOut error", e.Message);
                    }
                }
            }

            return View(model);
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
            // Get Product
            var product = _mapper.Map<Product, ProductViewModel>(await _mediator.Send(new GetProductByIdQuery()
            {
                ProductId = productId
            }));

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
                        var product = _mapper.Map<Product, ProductViewModel>(await _mediator.Send(new GetProductByIdQuery()
                        {
                            ProductId = productId
                        }));
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