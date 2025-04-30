using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using FitnessEquipmentShop.Controllers;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using System.Threading.Tasks;
using FitnessEquipmentShop.Web.ViewModel;

namespace FitnessEquipmentShop.UnitTests
{
    public class ReviewControllerTests
    {
        private Mock<IReviewService> _reviewServiceMock;
        private ReviewController _reviewController;

        [SetUp]
        public void SetUp()
        {
            _reviewServiceMock = new Mock<IReviewService>();
            _reviewController = new ReviewController(_reviewServiceMock.Object);
        }

        [Test]
        public async Task Index_ShouldReturnViewWithReviews()
        {
            // Arrange
            var reviews = new List<Review> { new Review { ProductId = 1, Rating = 5, Comment = "Great product!" } };
            _reviewServiceMock.Setup(r => r.GetReviewsByProductIdAsync(1)).ReturnsAsync(reviews);

            // Act
            var result = await _reviewController.Index(1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(reviews, viewResult.Model);
        }

        [Test]
        public async Task Add_ShouldReturnUnauthorizedIfUserNotAuthenticated()
        {
            // Arrange
            var review = new ReviewViewModel { ProductId = 1, Comment = "Nice!", Rating = 4 };

            // Act
            var result = await _reviewController.Add(review);

            // Assert
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(false, jsonResult.Value);
        }

        [Test]
        public async Task Edit_ShouldReturnUnauthorizedIfUserNotAdminOrOwner()
        {
            // Arrange
            var review = new Review { UserId = "user1", ProductId = 1, Comment = "Great!", Rating = 5 };
            _reviewServiceMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync(review);

            // Act
            var result = await _reviewController.Edit(1);

            // Assert
            var viewResult = result as UnauthorizedResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public async Task Delete_ShouldRedirectToProductDetailsAfterDeletion()
        {
            // Arrange
            var review = new Review { ProductId = 1 };
            _reviewServiceMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync(review);
            _reviewServiceMock.Setup(r => r.DeleteReviewAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _reviewController.Delete(1);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Details", redirectResult.ActionName);
            Assert.AreEqual("Product", redirectResult.ControllerName);
        }
    }
}
