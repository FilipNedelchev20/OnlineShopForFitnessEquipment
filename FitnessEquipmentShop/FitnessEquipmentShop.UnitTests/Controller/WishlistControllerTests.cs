using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using FitnessEquipmentShop.Controllers;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.UnitTests
{
    public class WishlistControllerTests
    {
        private Mock<IWishlistService> _wishlistServiceMock;
        private WishlistController _wishlistController;

        [SetUp]
        public void SetUp()
        {
            _wishlistServiceMock = new Mock<IWishlistService>();
            _wishlistController = new WishlistController(_wishlistServiceMock.Object);
        }

        [Test]
        public async Task Index_ShouldReturnViewWithWishlistItems()
        {
            // Arrange
            var wishlistItems = new List<Wishlist> { new Wishlist { ProductId = 1, UserId = "user1" } };
            _wishlistServiceMock.Setup(w => w.GetWishlistItemsAsync()).ReturnsAsync(wishlistItems);

            // Act
            var result = await _wishlistController.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(wishlistItems, viewResult.Model);
        }

        [Test]
        public async Task Add_ShouldRedirectToLoginIfUserNotAuthenticated()
        {
            // Act
            var result = await _wishlistController.Add(1);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Login", redirectResult.ActionName);
        }

        [Test]
        public async Task Remove_ShouldRedirectToIndexAfterRemoval()
        {
            // Arrange
            _wishlistServiceMock.Setup(w => w.RemoveFromWishlistAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _wishlistController.Remove(1);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }
    }
}
