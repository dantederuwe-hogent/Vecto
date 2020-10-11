using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Vecto.Api.Controllers;
using Vecto.Core.Interfaces;
using Xunit;

namespace Vecto.Tests.UnitTests.Api.Controllers
{
    public class ProfileControllerTest
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileController _sut;

        public ProfileControllerTest()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _sut = new ProfileController(_userRepository);
        }

        [Fact]
        public void GetProfile_UserLoggedIn_ReturnsUser()
        {
            // Arrange 
            var user = DummyData.UserFaker.Generate();

            _sut.ControllerContext = FakeControllerContext.GetLoggedInUserContextFor(user);

            _userRepository.GetBy(user.Email).Returns(user);

            // Act 
            var meResult = _sut.GetProfile();

            // Assert 
            meResult.Should().BeOfType<OkObjectResult>();
            _userRepository.Received().GetBy(user.Email);
        }

        [Fact]
        public void GetProfile_UserNotLoggedIn_ReturnsBadRequestResult()
        {
            // Arrange 
            _sut.ControllerContext = FakeControllerContext.NoLoggedInUserContext;

            // Act 
            var meResult = _sut.GetProfile();
            // Assert 
            meResult.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void GetProfile_UserLoggedIn_ReturnsBadRequestResult()
        {
            // Arrange 
            var user = DummyData.UserFaker.Generate();
            _sut.ControllerContext = FakeControllerContext.GetLoggedInUserContextFor(user);

            _userRepository.GetBy(user.Email).ReturnsNull();

            // Act 
            var meResult = _sut.GetProfile();

            // Assert 
            meResult.Should().BeOfType<BadRequestResult>();
        }
    }
}