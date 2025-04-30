using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using FitnessEquipmentShop.Controllers;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.UnitTests
{
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _categoryServiceMock;
        private CategoryController _categoryController;

        [SetUp]
        public void SetUp()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_categoryServiceMock.Object);
        }

        [Test]
        public async Task Index_ShouldReturnViewWithCategories()
        {
            // Arrange
            var categories = new List<Category> { new Category { Id = 1, Name = "Sports" } };
            _categoryServiceMock.Setup(c => c.GetAllCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _categoryController.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(categories, viewResult.Model);
        }

        [Test]
        public async Task Edit_ShouldReturnNotFoundIfCategoryDoesNotExist()
        {
            // Arrange
            _categoryServiceMock.Setup(c => c.GetCategoryByIdAsync(1)).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryController.Edit(1);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
