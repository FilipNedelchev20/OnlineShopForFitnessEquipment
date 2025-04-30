using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.UnitTests
{
    public class AdminServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private AdminService _adminService;

        [SetUp]
        public void SetUp()
        {
            _userManagerMock = new Mock<UserManager<User>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>();
            _adminService = new AdminService(_userManagerMock.Object, _roleManagerMock.Object);
        }

        [Test]
        public async Task AssignRoleAsync_ShouldReturnTrue_WhenRoleIsAssigned()
        {
            // Arrange
            var user = new User { Id = "1", Email = "test@example.com" };
            _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
            _roleManagerMock.Setup(r => r.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(u => u.AddToRoleAsync(user, "Admin")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _adminService.AssignRoleAsync("1", "Admin");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemoveAllRolesAsync_ShouldReturnTrue_WhenRolesAreRemoved()
        {
            var user = new User { Id = "1" };
            _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin", "User" });
            _userManagerMock.Setup(u => u.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);

            var result = await _adminService.RemoveAllRolesAsync("1");

            Assert.IsTrue(result);
        }
    }
}
