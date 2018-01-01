using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Data;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products)
                .Returns(new[]
                 {
                     new Product { ProductID = 1, Name = "P1" },
                     new Product { ProductID = 2, Name = "P2" },
                     new Product { ProductID = 3, Name = "P3" },
                     new Product { ProductID = 4, Name = "P4" },
                     new Product { ProductID = 5, Name = "P5" }
                 }.AsQueryable());

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            var result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            // Assert
            var prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }
    }
}