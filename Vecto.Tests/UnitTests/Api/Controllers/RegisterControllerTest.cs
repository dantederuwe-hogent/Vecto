using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Threading.Tasks;
using Vecto.Api.Controllers;
using Vecto.Application.Register;
using Vecto.Core.Entities;
using Vecto.Core.Interfaces;
using Vecto.Infrastructure.Data;
using Vecto.Tests.UnitTests.Helpers;
using Xunit;

namespace Vecto.Tests.UnitTests.Api.Controllers
{
    public class RegisterControllerTest
    {
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly UserManager<IdentityUser> _um = Substitute.For<FakeUserManager>();
        private readonly IConfiguration _config = FakeConfiguration.Get();
        private readonly IValidator<RegisterDTO> _registerValidator = Substitute.For<IValidator<RegisterDTO>>();

        private readonly RegisterController _sut;

        public RegisterControllerTest()
        {
            _sut = new RegisterController(_userRepository, _config, _um, _registerValidator);
        }

        [Fact]
        public async Task Register_GoodUser_ShouldRegisterAndReturnToken()
        {
            // Arrange
            var registerDTO = DummyData.RegisterDTOFaker.Generate();
            _um.CreateAsync(default, default).ReturnsForAnyArgs(IdentityResult.Success);
            _registerValidator.SetupPass();

            // Act          
            var result = await _sut.Register(registerDTO);

            // Assert
            result.Should().BeCreatedTokenResponse();
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
            _registerValidator.SetupPass();
            _userRepository.GetBy(user.Email).Returns(user); //!

            // Act     
            var secondTimeRegister = await _sut.Register(registerDTO);

            // Assert
            secondTimeRegister.Should().BeOfType<BadRequestResult>();
            _userRepository.DidNotReceive().Add(Arg.Any<User>());
            _userRepository.DidNotReceive().SaveChanges();
        }

        [Fact]
        public async Task Register_ValidationFailed_ShouldNotRegisterAndReturnsBadRequest()
        {
            // Arrange
            _registerValidator.SetupFail();

            // Act     
            var secondTimeRegister = await _sut.Register(default);

            // Assert
            secondTimeRegister.Should().BeOfType<BadRequestObjectResult>();
            _userRepository.DidNotReceive().Add(Arg.Any<User>());
            _userRepository.DidNotReceive().SaveChanges();
        }
    }
}
