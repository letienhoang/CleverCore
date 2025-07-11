using CleverCore.Application.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using CleverCore.Application.ViewModels.Product;
using CleverCore.Data.Enums;
using CleverCore.Utilities.Constants;
using CleverCore.WebApp.Controllers;
using CleverCore.WebApp.Models;
using CleverCore.WebApp.Test.Mock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace CleverCore.WebApp.Test.Controller
{
    public class CartControllerTest
    {
        private readonly Mock<IBillService> _mockBillService;
        private readonly Mock<IProductService> _mockProductService;
        public CartControllerTest()
        {
            _mockBillService = new Mock<IBillService>();
            _mockProductService = new Mock<IProductService>();
        }

        [Fact]
        public void Checkout_NullSession_RedirectResult()
        {
            // Arrange
            var data = new List<ShoppingCartViewModel>
            {
                new ShoppingCartViewModel()
                {
                    Color = null,
                    Size = null,
                    Product = new ProductViewModel()
                    {
                        Id = 1,
                        Name = "Test Product",
                        Price = 100,
                        Image = "test.jpg"
                    },
                    Quantity = 1,
                    Price = 100
                }
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new MockHttpSession();
            mockSession[CommonConstants.CartSession] = JsonConvert.SerializeObject(data);
            mockHttpContext.Setup(x => x.Session).Returns(mockSession);

            var controller = new CartController(_mockProductService.Object, _mockBillService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };
            // Act
            var result = controller.Checkout();
            // Assert
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void Checkout_PostValid_OkResult()
        {
            // Arrange
            var data = new List<ShoppingCartViewModel>
            {
                new ShoppingCartViewModel()
                {
                    Color = new ColorViewModel()
                    {
                        Id = 1,
                        Name = "Red",
                        Code = "#FF0000"
                    },
                    Size = new SizeViewModel()
                    {
                        Id = 1,
                        Name = "M"
                    },
                    Product = new ProductViewModel()
                    {
                        Id = 1,
                        Name = "Test Product",
                        Price = 100,
                        Image = "test.jpg"
                    },
                    Quantity = 1,
                    Price = 100
                }
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new MockHttpSession();
            mockSession[CommonConstants.CartSession] = JsonConvert.SerializeObject(data);
            mockHttpContext.Setup(x => x.Session).Returns(mockSession);

            var claims = new List<Claim>
            {
                new Claim("UserId", Guid.NewGuid().ToString()),
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockBillService.Setup(x => x.Create(It.IsAny<BillViewModel>()));
            _mockBillService.Setup(x => x.Save());
            var controller = new CartController(_mockProductService.Object, _mockBillService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };
            var viewModel = new CheckoutViewModel()
            {
                Carts = data,
                PaymentMethod = PaymentMethod.PaymentGateway,
                CustomerName = "Test User",
                CustomerAddress = "123 Test St",
                CustomerMobile = "1234567890",
            };

            // Act
            var result = controller.Checkout(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(true, viewResult.ViewData["Success"]);
        }

        [Fact]
        public void Checkout_ValidRequest_OkResult()
        {
            // Arrange
            var data = new List<ShoppingCartViewModel>
            {
                new ShoppingCartViewModel()
                {
                    Color = new ColorViewModel()
                    {
                        Id = 1,
                        Name = "Red",
                        Code = "#FF0000"
                    },
                    Size = new SizeViewModel()
                    {
                        Id = 1,
                        Name = "M"
                    },
                    Product = new ProductViewModel()
                    {
                        Id = 1,
                        Name = "Test Product",
                        Price = 100,
                        Image = "test.jpg"
                    },
                    Quantity = 1,
                    Price = 100
                }
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new MockHttpSession();
            mockSession[CommonConstants.CartSession] = JsonConvert.SerializeObject(data);

            mockHttpContext.Setup(x => x.Session).Returns(mockSession);
            var controller = new CartController(_mockProductService.Object, _mockBillService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Act
            var result = controller.Checkout();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CheckoutViewModel>(viewResult.Model);
            Assert.Single(model.Carts);
        }
    }
}
