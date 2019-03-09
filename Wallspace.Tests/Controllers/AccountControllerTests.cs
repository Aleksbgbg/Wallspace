namespace Wallspace.Tests.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Wallspace.Controllers;
    using Wallspace.Models;

    using Xunit;

    public class AccountControllerTests
    {
        private readonly Mock<UserManager<WallspaceUser>> _userManagerMock;

        public AccountControllerTests()
        {
            Mock<IUserStore<WallspaceUser>> userStoreMock = new Mock<IUserStore<WallspaceUser>>();

            _userManagerMock = new Mock<UserManager<WallspaceUser>>(userStoreMock.Object,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null);
        }

        [Fact]
        public async Task TestRegisterValidUser()
        {
            // Arrange
            _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<WallspaceUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            AccountController accountController = new AccountController(_userManagerMock.Object, null, null);

            RegistrationCredentials registrationCredentials = new RegistrationCredentials
            {
                Username = "Aleksbgbg",
                Email = "bgbg@aleks.dev",
                Password = "123456",
                RepeatPassword = "123456",
                PhoneNumber = "123456"
            };

            _userManagerMock.Object.UserValidators.Add(new UserValidator<WallspaceUser>());
            _userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<WallspaceUser>());

            // Act
            IActionResult registrationResult = await accountController.SignUp(registrationCredentials);

            // Assert
            Assert.IsType<OkResult>(registrationResult);
        }

        [Fact]
        public async Task TestRegisterInvalidCredentials()
        {
            // Arrange
            AccountController accountController = new AccountController(_userManagerMock.Object, null, null);

            // Invalid credentials simulation
            accountController.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            IActionResult registrationResult = await accountController.SignUp(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(registrationResult);
        }
    }
}