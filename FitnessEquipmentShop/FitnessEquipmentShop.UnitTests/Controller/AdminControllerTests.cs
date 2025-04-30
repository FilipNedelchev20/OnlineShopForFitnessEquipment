using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Controllers;
using FitnessEquipmentShop.Data.Models.Entities;
using System.Threading.Tasks;
using FitnessEquipmentShop.Web.Controllers;

namespace FitnessEquipmentShop.UnitTests
{
    public class AdminControllerTests
    {
        private Mock<IAdminService> _adminServiceMock;
        private Mock<IOrderService> _orderServiceMock;
        private Mock<UserManager<User>> _userManagerMock;
        private AdminController _adminController;

        [SetUp]
        public void SetUp()
        {
            _adminServiceMock = new Mock<IAdminService>();
            _orderServiceMock = new Mock<IOrderService>();
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _adminController = new AdminController(_adminServiceMock.Object, _userManagerMock.Object, _orderServiceMock.Object);
        }

        [Test]
        public async Task Index_ShouldReturnViewWithOrders()
        {
            // Arrange
            var orders = new List<Order> { new Order { Id = 1, Status = "Pending" } };
            _orderServiceMock.Setup(o => o.GetAllOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _adminController.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(orders, viewResult.Model);
        }

        [Test]
        public async Task AssignRole_ShouldAssignRoleAndRedirect()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            _userManagerMock.Setup(u => u.FindByEmailAsync("test@example.com")).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.AddToRoleAsync(user, "Admin")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _adminController.AssignRole("test@example.com", "Admin");

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("ManageUsers", redirectResult.ActionName);
        }
    }
}
