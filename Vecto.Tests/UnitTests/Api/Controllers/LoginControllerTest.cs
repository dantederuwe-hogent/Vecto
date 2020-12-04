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
using Vecto.Infrastructure;
using Vecto.Infrastructure.Data;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Vecto.Tests.UnitTests.Api.Controllers
{
    public class LoginControllerTest
    {
        private readonly SignInManager<IdentityUser> _sim;
        private readonly LoginController _sut;
        private readonly UserManager<IdentityUser> _um;
        private readonly IConfiguration _config;
        private readonly IValidator<LoginDTO> _loginValidator;

        public LoginControllerTest()
        {
            _sim = Substitute.For<FakeSignInManager>();
            _um = Substitute.For<FakeUserManager>();
            _config = FakeConfiguration.Get();
            _loginValidator = Substitute.For<IValidator<LoginDTO>>();

            _sut = new LoginController(_loginValidator, _um, _sim, _config);
        }

        [Fact]

        public async Task Login_SuccessfulLogin_ShouldReturnToken()
        {
            // Arrange
            var loginDTO = DummyData.LoginDTOFaker.Generate();

            _loginValidator.SetupPass();
            _um.FindByNameAsync(loginDTO.Email).Returns(new IdentityUser() { UserName = loginDTO.Email, Email = loginDTO.Email });

            _sim.CheckPasswordSignInAsync(Arg.Any<IdentityUser>(), Arg.Any<string>(), Arg.Any<bool>())
                .Returns(Task.FromResult(SignInResult.Success));

            //Act
            var login = await _sut.Login(loginDTO);

            //Assert
            login.Should().BeOfType<OkObjectResult>()
                .Which.Value.ToString().Should().MatchRegex("^[A-Za-z0-9-_=]+\\.[A-Za-z0-9-_=]+\\.?[A-Za-z0-9-_.+/=]*$");

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