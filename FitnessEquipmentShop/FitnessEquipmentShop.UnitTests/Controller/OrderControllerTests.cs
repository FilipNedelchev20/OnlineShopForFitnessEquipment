using Moq;
using NUnit.Framework;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Controllers;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.UnitTests
{
    public class OrderControllerTests
    {
        private Mock<IOrderService> _orderServiceMock;
        private Mock<UserManager<User>> _userManagerMock;
        private OrderController _orderController;

        [SetUp]
        public void SetUp()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _orderController = new OrderController(_orderServiceMock.Object, _userManagerMock.Object);
        }

        [Test]
        public async Task Index_ShouldReturnViewWithOrders()
        {
            // Arrange
            var orders = new List<Order> { new Order { Id = 1, Status = "Pending" } };
            _orderServiceMock.Setup(o => o.GetOrdersByUserIdAsync("user1")).ReturnsAsync(orders);

            // Act
            var result = await _orderController.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(orders, viewResult.Model);
        }
    }
}
