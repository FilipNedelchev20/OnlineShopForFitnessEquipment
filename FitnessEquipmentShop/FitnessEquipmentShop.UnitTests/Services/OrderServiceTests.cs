using NUnit.Framework;
using Moq;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Services.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace FitnessEquipmentShop.UnitTests
{
    public class OrderServiceTests
    {
        private Mock<ApplicationDbContext> _contextMock;
        private Mock<IEmailService> _emailServiceMock;
        private Mock<UserManager<User>> _userManagerMock;
        private OrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            _contextMock = new Mock<ApplicationDbContext>();
            _emailServiceMock = new Mock<IEmailService>();
            _userManagerMock = new Mock<UserManager<User>>();
            _orderService = new OrderService(_contextMock.Object, _emailServiceMock.Object, _userManagerMock.Object);
        }

        [Test]
        public async Task PlaceOrderAsync_ShouldThrowException_WhenCartIsEmpty()
        {
            _contextMock.Setup(c => c.CartItems.Where(It.IsAny<Func<CartItem, bool>>())).Returns(new List<CartItem>());

            Assert.ThrowsAsync<Exception>(async () => await _orderService.PlaceOrderAsync("user1", new Address()));
        }
    }
}
