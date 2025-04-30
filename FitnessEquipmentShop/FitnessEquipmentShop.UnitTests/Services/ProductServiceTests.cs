using NUnit.Framework;
using Moq;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessEquipmentShop.Data;

namespace FitnessEquipmentShop.UnitTests
{
    public class ProductServiceTests
    {
        private Mock<ApplicationDbContext> _contextMock;
        private ProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _contextMock = new Mock<ApplicationDbContext>();
            _productService = new ProductService(_contextMock.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ShouldReturnListOfProducts()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Product1" } };
            _contextMock.Setup(c => c.Products.ToList()).Returns(products);

            var result = await _productService.GetAllProductsAsync();

            Assert.AreEqual(1, result.Count());
        }
    }
}
