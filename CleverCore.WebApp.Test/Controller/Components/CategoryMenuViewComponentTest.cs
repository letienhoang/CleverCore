using CleverCore.Application.Interfaces;
using CleverCore.WebApp.Controllers.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleverCore.WebApp.Test.Controller.Components
{
    public class CategoryMenuViewComponentTest
    {
        [Fact]
        public async Task Invoke_ValidInput_ResultOk()
        {
            // Arrange
            var mockProductCategoryService = new Mock<IProductCategoryService>();
            mockProductCategoryService.Setup(x => x.GetAll())
                .Returns(new List<CleverCore.Application.ViewModels.Product.ProductCategoryViewModel>
                {
                    new CleverCore.Application.ViewModels.Product.ProductCategoryViewModel { Id = 1, Name = "Category 1" },
                    new CleverCore.Application.ViewModels.Product.ProductCategoryViewModel { Id = 2, Name = "Category 2" }
                });
            var cache = new MemoryCache(new MemoryCacheOptions());
            var viewComponent = new CategoryMenuViewComponent(mockProductCategoryService.Object, cache);

            // Act
            var result = await viewComponent.InvokeAsync();

            // Assert
            Assert.IsAssignableFrom<IViewComponentResult>(result);
        }
    }
}