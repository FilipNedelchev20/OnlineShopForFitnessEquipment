using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FitnessEquipmentShop.Controllers;
using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FitnessEquipmentShop.UnitTests
{
    public class ProfileControllerTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<SignInManager<User>> _signInManagerMock;
        private Mock<IWebHostEnvironment> _webHostEnvMock;
        private ProfileController _profileController;

        [SetUp]
        public void SetUp()
        {
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>());
            _webHostEnvMock = new Mock<IWebHostEnvironment>();
            _profileController = new ProfileController(_userManagerMock.Object, _signInManagerMock.Object, _webHostEnvMock.Object);
        }

        [Test]
        public async Task Index_ShouldReturnViewWithProfileData()
        {
            // Arrange
            var user = new User { FullName = "John Doe", Email = "john.doe@example.com", ProfilePicturePath = "/images/profiles/johndoe.jpg" };
            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);

            // Act
            var result = await _profileController.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(user, viewResult.Model);
        }
    }
}
