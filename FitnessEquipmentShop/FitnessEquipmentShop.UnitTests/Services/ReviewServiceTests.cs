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
    public class ReviewServiceTests
    {
        private Mock<ApplicationDbContext> _contextMock;
        private Mock<DbSet<Review>> _mockReviewSet;
        private ReviewService _reviewService;

        [SetUp]
        public void SetUp()
        {
            _contextMock = new Mock<ApplicationDbContext>();

            // Mocking the DbSet<Review>
            _mockReviewSet = new Mock<DbSet<Review>>();

            // Setting up DbSet for Reviews
            _contextMock.Setup(c => c.Reviews).Returns(_mockReviewSet.Object);

            _reviewService = new ReviewService(_contextMock.Object);
        }

        [Test]
        public async Task AddReviewAsync_ShouldAddReview()
        {
            // Arrange
            var review = new Review { ProductId = 1, UserId = "user1", Rating = 5, Comment = "Great product!" };

            // Mocking AddAsync to simulate adding a new Review
            var mockEntityEntry = new Mock<EntityEntry<Review>>();
            _mockReviewSet.Setup(r => r.AddAsync(It.IsAny<Review>(), default)).ReturnsAsync(mockEntityEntry.Object);

            // Mocking SaveChangesAsync to return a successful Task
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1); // 1 indicates one row affected

            // Act
            await _reviewService.AddReviewAsync(review);

            // Assert
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
            _mockReviewSet.Verify(r => r.AddAsync(It.IsAny<Review>(), default), Times.Once);
        }
    }
}
