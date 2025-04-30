using NUnit.Framework;
using Moq;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FitnessEquipmentShop.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FitnessEquipmentShop.UnitTests
{
    public class WishlistServiceTests
    {
        private Mock<ApplicationDbContext> _contextMock;
        private Mock<DbSet<Product>> _mockProductSet;
        private Mock<DbSet<Wishlist>> _mockWishlistSet;
        private WishlistService _wishlistService;

        [SetUp]
        public void SetUp()
        {
            _contextMock = new Mock<ApplicationDbContext>();

            // Mocking the DbSets
            _mockProductSet = new Mock<DbSet<Product>>();
            _mockWishlistSet = new Mock<DbSet<Wishlist>>();

            // Setting up DbSet for Products
            _contextMock.Setup(c => c.Products).Returns(_mockProductSet.Object);
            // Setting up DbSet for Wishlist
            _contextMock.Setup(c => c.Wishlists).Returns(_mockWishlistSet.Object);

            _wishlistService = new WishlistService(_contextMock.Object);
        }

        [Test]
        public async Task AddToWishlistAsync_ShouldAddItem_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };

            // Mocking the FindAsync to return the product when the ID matches
            _mockProductSet.Setup(p => p.FindAsync(1)).ReturnsAsync(product);

            // Mocking AddAsync to simulate adding a new Wishlist item
            var mockEntityEntry = new Mock<EntityEntry<Wishlist>>();
            _mockWishlistSet.Setup(w => w.AddAsync(It.IsAny<Wishlist>(), default)).ReturnsAsync(mockEntityEntry.Object);

            // Mocking SaveChangesAsync to return a successful Task
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1); // 1 indicates one row affected

            // Act
            await _wishlistService.AddToWishlistAsync(1, "user1");

            // Assert
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
            _mockWishlistSet.Verify(w => w.AddAsync(It.IsAny<Wishlist>(), default), Times.Once);
        }
    }
}
