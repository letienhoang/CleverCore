﻿using CleverCore.Application.Interfaces;
using CleverCore.Application.ViewModels.Product;
using CleverCore.Data.Enums;
using CleverCore.Utilities.Constants;
using CleverCore.WebApp.Extensions;
using CleverCore.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleverCore.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IBillService _billService;
        private readonly IProductService _productService;

        public CartController(IProductService productService, IBillService billService)
        {
            _productService = productService;
            _billService = billService;
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("checkout.html", Name = "Checkout")]
        [HttpGet]
        public IActionResult Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            List<ShoppingCartViewModel> session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session.Any(x => x.Color == null || x.Size == null))
            {
                return Redirect("/cart.html");
            }

            model.Carts = session;
            return View(model);
        }

        [Route("checkout.html", Name = "Checkout")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            List<ShoppingCartViewModel> session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);

            if (ModelState.IsValid)
            {
                if (session != null)
                {
                    List<BillDetailViewModel> details = new List<BillDetailViewModel>();
                    foreach (ShoppingCartViewModel item in session)
                    {
                        details.Add(new BillDetailViewModel
                        {
                            Product = item.Product,
                            Price = item.Price,
                            ColorId = item.Color.Id,
                            SizeId = item.Size.Id,
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id
                        });
                    }

                    BillViewModel billViewModel = new BillViewModel
                    {
                        CustomerMobile = model.CustomerMobile,
                        BillStatus = BillStatus.New,
                        CustomerAddress = model.CustomerAddress,
                        CustomerName = model.CustomerName,
                        CustomerMessage = model.CustomerMessage,
                        BillDetails = details
                    };
                    if (User.Identity.IsAuthenticated)
                    {
                        billViewModel.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
                    }

                    _billService.Create(billViewModel);
                    try
                    {
                        _billService.Save();

                        //var content = await _viewRenderService.RenderToStringAsync("Cart/_BillMail", billViewModel);
                        //Send mail
                        //await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "New bill from Panda Shop", content);
                        ViewData["Success"] = true;
                    }
                    catch (Exception ex)
                    {
                        ViewData["Success"] = false;
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            model.Carts = session;
            return View(model);
        }

        #region AJAX Request

        /// <summary>
        ///     Get list item
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            List<ShoppingCartViewModel> session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session == null)
            {
                session = new List<ShoppingCartViewModel>();
            }

            return new OkObjectResult(session);
        }

        /// <summary>
        ///     Remove all products in cart
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

        /// <summary>
        ///     Add product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, int color, int size)
        {
            //Get product detail
            ProductViewModel product = _productService.GetById(productId);

            //Get session with item list from cart
            List<ShoppingCartViewModel> session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //Convert string to list object
                bool hasChanged = false;

                //Check exist with item product id
                if (session.Any(x => x.Product.Id == productId))
                {
                    foreach (ShoppingCartViewModel item in session)
                        //Update quantity for product if match product id
                        if (item.Product.Id == productId)
                        {
                            item.Quantity += quantity;
                            item.Price = product.PromotionPrice ?? product.Price;
                            hasChanged = true;
                        }
                }
                else
                {
                    session.Add(new ShoppingCartViewModel
                    {
                        Product = product,
                        Quantity = quantity,
                        Color = _billService.GetColor(color),
                        Size = _billService.GetSize(size),
                        Price = product.PromotionPrice ?? product.Price
                    });
                    hasChanged = true;
                }

                //Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                //Add new cart
                List<ShoppingCartViewModel> cart = new List<ShoppingCartViewModel>();
                cart.Add(new ShoppingCartViewModel
                {
                    Product = product,
                    Quantity = quantity,
                    Color = _billService.GetColor(color),
                    Size = _billService.GetSize(size),
                    Price = product.PromotionPrice ?? product.Price
                });
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }

            return new OkObjectResult(productId);
        }

        /// <summary>
        ///     Remove a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int productId)
        {
            List<ShoppingCartViewModel> session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (ShoppingCartViewModel item in session)
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }

                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }

            return new EmptyResult();
        }

        /// <summary>
        ///     Update product quantity
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IActionResult UpdateCart(int productId, int quantity, int color, int size)
        {
            List<ShoppingCartViewModel> session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (ShoppingCartViewModel item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        ProductViewModel product = _productService.GetById(productId);
                        item.Product = product;
                        item.Size = _billService.GetSize(size);
                        item.Color = _billService.GetColor(color);
                        item.Quantity = quantity;
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }

                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }

            return new EmptyResult();
        }

        [HttpGet]
        public IActionResult GetColors()
        {
            List<ColorViewModel> colors = _billService.GetColors();
            return new OkObjectResult(colors);
        }

        [HttpGet]
        public IActionResult GetSizes()
        {
            List<SizeViewModel> sizes = _billService.GetSizes();
            return new OkObjectResult(sizes);
        }

        #endregion AJAX Request
    }
}