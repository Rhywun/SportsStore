using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
            Assert.NotNull(result);
            var products = result.Products.ToArray();
            Assert.True(products.Length == 2);
            Assert.Equal("P4", products[0].Name);
            Assert.Equal("P5", products[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
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
            Assert.NotNull(result);
            var pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products)
                .Returns(new[]
                 {
                     new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                     new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                     new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                     new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                     new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
                 }.AsQueryable());

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            var result = ((ProductsListViewModel) controller.List("Cat2").ViewData.Model)
                        .Products.ToArray();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products)
                .Returns((new[]
                 {
                     new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                     new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                     new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                     new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                     new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
                 }).AsQueryable());

            var target = new ProductController(mock.Object) { PageSize = 3 };

            ProductsListViewModel GetModel(ViewResult result) =>
                result?.ViewData?.Model as ProductsListViewModel;

            // Action
            int? res1 = GetModel(target.List("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = GetModel(target.List("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel(target.List("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(target.List(null))?.PagingInfo.TotalItems;

            // Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
