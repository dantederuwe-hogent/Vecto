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
using Vecto.Infrastructure;
using Vecto.Infrastructure.Data;
using Xunit;

namespace Vecto.Tests.UnitTests.Api.Controllers
{
    public class RegisterControllerTest
    {
        private readonly IUserRepository _userRepository;
        private readonly RegisterController _sut;
        private readonly UserManager<IdentityUser> _um;
        private readonly IConfiguration _config;
        private readonly IValidator<RegisterDTO> _registerValidator;

        public RegisterControllerTest()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _um = Substitute.For<FakeUserManager>();
            _config = FakeConfiguration.Get();
            _registerValidator = Substitute.For<IValidator<RegisterDTO>>();

            _sut = new RegisterController(_userRepository, _config, _um, _registerValidator);

        }

        [Fact]
        public async Task Register_GoodUser_ShouldRegisterAndReturnToken()
        {
            // Arrange
            var registerDTO = DummyData.RegisterDTOFaker.Generate();
            _um.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);
            _registerValidator.SetupPass();

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

            _registerValidator.SetupPass();
            _userRepository.GetBy(user.Email).Returns(user); //already registered

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
            var secondTimeRegister = await _sut.Register(new RegisterDTO());

            // Assert
            secondTimeRegister.Should().BeOfType<BadRequestObjectResult>();
            _userRepository.DidNotReceive().Add(Arg.Any<User>());
            _userRepository.DidNotReceive().SaveChanges();
        }
    }
}