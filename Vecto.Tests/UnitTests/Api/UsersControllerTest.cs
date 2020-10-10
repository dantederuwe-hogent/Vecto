using DamianTourBackend.Tests.UnitTests.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Threading.Tasks;
using Vecto.Api.Controllers;
using Vecto.Core.Entities;
using Vecto.Core.Interfaces;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Vecto.Tests.UnitTests.Api
{
    public class UsersControllerTest
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<IdentityUser> _sim;
        private readonly UsersController _sut;
        private readonly UserManager<IdentityUser> _um;
        private readonly IConfiguration _config;

        public UsersControllerTest()
        {
            _userRepository = Substitute.For<IUserRepository>();

            _sim = Substitute.For<FakeSignInManager>();
            _um = Substitute.For<FakeUserManager>();
            _config = FakeConfiguration.Get();

            _sut = new UsersController(_userRepository, _config, _sim, _um);

        }
        #region Register Tests

        [Fact]
        public async Task Register_GoodUser_ShouldRegisterAndReturnToken()
        {
            // Arrange
            var registerDTO = DummyData.RegisterDTOFaker.Generate();
            _um.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            // Act          
            var result = await _sut.Register(registerDTO);

            // Assert
            result.Should().BeOfType<CreatedResult>()
                .Which.Value.ToString().Should().MatchRegex("^[A-Za-z0-9-_=]+\\.[A-Za-z0-9-_=]+\\.?[A-Za-z0-9-_.+/=]*$");

            _userRepository.Received().Add(Arg.Any<User>());
            _userRepository.Received().SaveChanges();

        }

        [Fact]
        public async Task Register_EmailAlreadyRegistered_ShouldNotRegisterAndReturnsBadRequest()
        {
            // Arrange
            var user = DummyData.UserFaker.Generate();
            var registerDTO = DummyData.RegisterDTOFaker.Generate();
            registerDTO.Email = user.Email;

            _userRepository.GetBy(user.Email).Returns(user); //already registered

            // Act     
            var secondTimeRegister = await _sut.Register(registerDTO);

            // Assert
            secondTimeRegister.Should().BeOfType<BadRequestResult>();
            _userRepository.DidNotReceive().Add(Arg.Any<User>());
            _userRepository.DidNotReceive().SaveChanges();
        }

        #endregion

        #region Login Tests
        [Fact]

        public async Task Login_SuccessfulLogin_ShouldReturnToken()

        {
            // Arrange
            var loginDTO = DummyData.LoginDTOFaker.Generate();

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
            _um.FindByNameAsync(loginDTO.Email).ReturnsNull();

            //Act
            var login = await _sut.Login(loginDTO);

            //Assert
            login.Should().BeOfType<BadRequestResult>();
        }
        #endregion
    }
}