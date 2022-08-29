﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void CanAddCartItem()
        {
            //setup
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();

            var httpContext = new MockHttpContext();

            ICartService cartService = new CartService(products, carts);
            var controller = new CartController(cartService);
            controller.ControllerContext = 
                new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //act
            //cartService.AddToCart(httpContext, "1");
            controller.AddToCart("1");

            Cart cart = carts.Collection().FirstOrDefault();

            //assert
            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.CartItems.Count);
            Assert.AreEqual("1", cart.CartItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            //setup
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();

            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            Cart cart = new Cart();
            cart.CartItems.Add(new CartItem() { ProductId = "1", Quantity = 2 });
            cart.CartItems.Add(new CartItem() { ProductId = "2", Quantity = 3 });
            carts.Insert(cart);

            ICartService cartService = new CartService(products, carts);

            var controller = new CartController(cartService);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceCart") { Value = cart.Id });
            controller.ControllerContext =
               new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.CartSummary() as PartialViewResult;
            var cartSummary = (CartSummaryViewModel)result.ViewData.Model;

          
            Assert.AreEqual(5, cartSummary.CartCount);
            Assert.AreEqual(35.00m, cartSummary.CartTotal);
        }
    }
}
