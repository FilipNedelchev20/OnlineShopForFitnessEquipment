using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Controllers;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.UnitTests
{
    public class CartControllerTests
    {
        private Mock<ICartService> _cartServiceMock;
        private Mock<UserManager<User>> _userManagerMock;
        private CartController _cartController;

        [SetUp]
        public void SetUp()
        {
            _cartServiceMock = new Mock<ICartService>();
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _cartController = new CartController(_cartServiceMock.Object, _userManagerMock.Object);
        }

        [Test]
        public async Task Index_ShouldReturnViewWithCartItems()
        {
            // Arrange
            var cartItems = new List<CartItem> { new CartItem { Id = 1, Quantity = 1 } };
            _cartServiceMock.Setup(c => c.GetCartItemsAsync()).ReturnsAsync(cartItems);

            // Act
            var result = await _cartController.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(cartItems, viewResult.Model);
        }

        [Test]
        public async Task Add_ShouldRedirectToLoginIfNotAuthenticated()
        {
            // Act
            var result = await _cartController.Add(1);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Login", redirectResult.ActionName);
        }
    }
}
