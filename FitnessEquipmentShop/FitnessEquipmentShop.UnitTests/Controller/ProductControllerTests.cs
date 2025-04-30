using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Controllers;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FitnessEquipmentShop.UnitTests
{
    public class ProductControllerTests
    {
        private Mock<IProductService> _productServiceMock;
        private Mock<ICategoryService> _categoryServiceMock;
        private ProductController _productController;

        [SetUp]
        public void SetUp()
        {
            _productServiceMock = new Mock<IProductService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _productController = new ProductController(_productServiceMock.Object, _categoryServiceMock.Object);
        }

        [Test]
        public async Task Index_ShouldReturnViewWithProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Treadmill", Price = 500 } };
            _productServiceMock.Setup(p => p.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _productController.Index(null, null, null, null, "");

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(products, viewResult.Model);
        }

        [Test]
        public async Task Details_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Treadmill", Price = 500 };
            _productServiceMock.Setup(p => p.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _productController.Details(1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(product, viewResult.Model);
        }
    }
}
