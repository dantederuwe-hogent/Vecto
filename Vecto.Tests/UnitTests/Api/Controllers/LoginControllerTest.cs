using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Threading.Tasks;
using Vecto.Api.Controllers;
using Vecto.Application.Login;
using Vecto.Infrastructure.Data;
using Vecto.Tests.UnitTests.Helpers;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Vecto.Tests.UnitTests.Api.Controllers
{
    public class LoginControllerTest
    {
        private readonly SignInManager<IdentityUser> _sim = Substitute.For<FakeSignInManager>();
        private readonly UserManager<IdentityUser> _um = Substitute.For<FakeUserManager>();
        private readonly IConfiguration _config = FakeConfiguration.Get();
        private readonly IValidator<LoginDTO> _loginValidator = Substitute.For<IValidator<LoginDTO>>();

        private readonly LoginController _sut;

        public LoginControllerTest()
        {
            _sut = new LoginController(_loginValidator, _um, _sim, _config);
        }

        [Fact]
        public async Task Login_SuccessfulLogin_ShouldReturnToken()
        {
            // Arrange
            var loginDTO = DummyData.LoginDTOFaker.Generate();

            _loginValidator.SetupPass();
            _um.FindByNameAsync(loginDTO.Email).Returns(loginDTO.ToIdentityUser());
            _sim.CheckPasswordSignInAsync(default, default, default)
                .ReturnsForAnyArgs(Task.FromResult(SignInResult.Success));

            //Act
            var login = await _sut.Login(loginDTO);

            //Assert
            login.Should().BeOkTokenResponse();
        }

        [Fact]
        public async Task Login_UserNotFoundOrRegistered_ShouldReturnBadRequest()
        {
            // Arrange
            var loginDTO = DummyData.LoginDTOFaker.Generate();
            _loginValidator.SetupPass();
            _um.FindByNameAsync(loginDTO.Email).ReturnsNull();

            //Act
            var login = await _sut.Login(loginDTO);

            //Assert
            login.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Login_ValidationFailed_ShouldNotLoginAndReturnBadRequest()
        {
            // Arrange
            _loginValidator.SetupFail();

            // Act     
            var loginFail = await _sut.Login(new LoginDTO());

            // Assert
            loginFail.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}