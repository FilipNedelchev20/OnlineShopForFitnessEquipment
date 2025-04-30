using NUnit.Framework;
using Moq;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using FitnessEquipmentShop.Data;

namespace FitnessEquipmentShop.UnitTests
{
    public class CategoryServiceTests
    {
        private Mock<ApplicationDbContext> _contextMock;
        private Mock<DbSet<Category>> _mockCategorySet;
        private CategoryService _categoryService;

        [SetUp]
        public void SetUp()
        {
            _contextMock = new Mock<ApplicationDbContext>();

            // Mocking the DbSet<Category>
            _mockCategorySet = new Mock<DbSet<Category>>();

            // Setting up DbSet for Categories
            _contextMock.Setup(c => c.Categories).Returns(_mockCategorySet.Object);

            _categoryService = new CategoryService(_contextMock.Object);
        }

        [Test]
        public async Task CreateCategoryAsync_ShouldAddCategory()
        {
            // Arrange
            var category = new Category { Name = "Category1" };

            // Mocking AddAsync to simulate adding a new Category
            var mockEntityEntry = new Mock<EntityEntry<Category>>();
            _mockCategorySet.Setup(c => c.AddAsync(It.IsAny<Category>(), default)).ReturnsAsync(mockEntityEntry.Object);

            // Mocking SaveChangesAsync to return a successful Task
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1); // 1 indicates one row affected

            // Act
            await _categoryService.CreateCategoryAsync(category);

            // Assert
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
            _mockCategorySet.Verify(c => c.AddAsync(It.IsAny<Category>(), default), Times.Once);
        }
    }
}
