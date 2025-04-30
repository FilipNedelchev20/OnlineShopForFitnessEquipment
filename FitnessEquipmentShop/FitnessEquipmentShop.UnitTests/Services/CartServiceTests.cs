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
    public class CartServiceTests
    {
        private Mock<ApplicationDbContext> _contextMock;
        private Mock<DbSet<Product>> _mockProductSet;
        private Mock<DbSet<CartItem>> _mockCartItemSet;
        private CartService _cartService;

        [SetUp]
        public void SetUp()
        {
            _contextMock = new Mock<ApplicationDbContext>();

            // Mocking DbSets
            _mockProductSet = new Mock<DbSet<Product>>();
            _mockCartItemSet = new Mock<DbSet<CartItem>>();

            // Setting up DbSets for Products and CartItems
            _contextMock.Setup(c => c.Products).Returns(_mockProductSet.Object);
            _contextMock.Setup(c => c.CartItems).Returns(_mockCartItemSet.Object);

            _cartService = new CartService(_contextMock.Object);
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddItem_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1", Price = 100 };
            var cartItem = new CartItem { ProductId = 1, UserId = "user1", Quantity = 1 };

            // Mocking FindAsync to return the product
            _mockProductSet.Setup(p => p.FindAsync(1)).ReturnsAsync(product);

            // Mocking AddAsync to return a mocked EntityEntry (mocking the entity entry returned by EF)
            var mockEntityEntry = new Mock<EntityEntry<CartItem>>();
            _mockCartItemSet.Setup(c => c.AddAsync(It.IsAny<CartItem>(), default)).ReturnsAsync(mockEntityEntry.Object);

            // Mocking SaveChangesAsync to return 1 (indicating one row was affected)
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _cartService.AddToCartAsync(1, "user1");

            // Assert
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);  // Verify SaveChangesAsync was called
            _mockCartItemSet.Verify(c => c.AddAsync(It.IsAny<CartItem>(), default), Times.Once);  // Verify AddAsync was called
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldRemoveItem_WhenCartItemExists()
        {
            // Arrange
            var cartItem = new CartItem { Id = 1, ProductId = 1, UserId = "user1" };

            // Mocking FindAsync to return the cart item
            _mockCartItemSet.Setup(c => c.FindAsync(1)).ReturnsAsync(cartItem);

            // Mocking Remove to simulate removing the cart item (doesn't return anything, but we simulate this behavior)
            _mockCartItemSet.Setup(c => c.Remove(It.IsAny<CartItem>())).Callback<CartItem>((item) => { /* simulate remove */ });

            // Mocking SaveChangesAsync to return 1 (indicating one row was affected)
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _cartService.RemoveFromCartAsync(1);

            // Assert
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);  // Verify SaveChangesAsync was called
            _mockCartItemSet.Verify(c => c.Remove(It.IsAny<CartItem>()), Times.Once);  // Verify Remove was called
        }
    }
}
