using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Threading.Tasks;
using Vecto.Api.Controllers;
using Vecto.Application.Profile;
using Vecto.Core.Interfaces;
using Xunit;

namespace Vecto.Tests.UnitTests.Api.Controllers
{
    public class ProfileControllerTest
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileController _sut;
        private readonly IValidator<ProfileDTO> _profileValidator;
        private readonly UserManager<IdentityUser> _um;

        public ProfileControllerTest()
        {
            _profileValidator = Substitute.For<IValidator<ProfileDTO>>();
            _um = Substitute.For<FakeUserManager>();
            _userRepository = Substitute.For<IUserRepository>();
            _sut = new ProfileController(_userRepository, _um, _profileValidator);
        }

        [Fact]
        public void GetProfile_UserLoggedIn_ReturnsUser()
        {
            // Arrange 
            var user = DummyData.UserFaker.Generate();

            _sut.ControllerContext = FakeControllerContext.GetLoggedInUserContextFor(user);

            _userRepository.GetBy(user.Email).Returns(user);

            // Act 
            var result = _sut.Get();

            // Assert 
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(user.MapToDTO());
            _userRepository.Received().GetBy(user.Email);
        }

        [Fact]
        public void GetProfile_UserNotLoggedIn_ReturnsBadRequestResult()
        {
            // Arrange 
            _sut.ControllerContext = FakeControllerContext.NoLoggedInUserContext;

            // Act 
            var result = _sut.Get();
            // Assert 
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void GetProfile_UserLoggedIn_ReturnsBadRequestResult()
        {
            // Arrange 
            var user = DummyData.UserFaker.Generate();
            _sut.ControllerContext = FakeControllerContext.GetLoggedInUserContextFor(user);

            _userRepository.GetBy(user.Email).ReturnsNull();

            // Act 
            var result = _sut.Get();

            // Assert 
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteProfile_ProfileDeleted_Succes()
        {
            // Arrange 
            var user = DummyData.UserFaker.Generate();
            var identityUser = user.MapToIdentityUser();

            _um.FindByNameAsync(identityUser.Email).Returns(identityUser);
            _um.DeleteAsync(identityUser).Returns(IdentityResult.Success);

            _sut.ControllerContext = FakeControllerContext.GetLoggedInUserContextFor(user);

            _userRepository.GetBy(user.Email).Returns(user);

            // Act 
            var result = await _sut.Delete();

            // Assert 
            result.Should().BeOfType<OkResult>();
            _userRepository.Received().Delete(user);
            _userRepository.Received().SaveChanges();
        }

        [Fact]
        public async Task DeleteProfile_UserNotFound_FailsAndReturnsBadRequestResult()
        {
            // Arrange 
            var user = DummyData.UserFaker.Generate();

            _sut.ControllerContext = FakeControllerContext.GetLoggedInUserContextFor(user);
            _userRepository.GetBy(user.Email).ReturnsNull();

            // Act 
            var result = await _sut.Delete();

            // Assert 
            result.Should().BeOfType<BadRequestResult>();
        }
        [Fact]
        public async Task DeleteProfile_UserNotLoggedIn_FailsAndReturnsUnauthorizedResult()
        {
            // Arrange 
            var user = DummyData.UserFaker.Generate();

            _sut.ControllerContext = FakeControllerContext.NoLoggedInUserContext;
            _userRepository.GetBy(user.Email).ReturnsNull();

            // Act 
            var result = await _sut.Delete();

            // Assert 
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task UpdateProfile_ProfileUpdated_Success()
        {
            // Arrange 
            var updateProfileDTO = DummyData.ProfileDTOFaker.Generate();
            var user = DummyData.UserFaker.Generate();
            var identityUser = user.MapToIdentityUser();

            _profileValidator.SetupPass();
            _um.FindByNameAsync(identityUser.Email).Returns(identityUser);
            _um.UpdateAsync(identityUser).Returns(IdentityResult.Success);
            _sut.ControllerContext = FakeControllerContext.GetLoggedInUserContextFor(user);
            _userRepository.GetBy(user.Email).Returns(user);

            // Act 
            var result = await _sut.Update(updateProfileDTO);

            // Assert 
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(user.MapToDTO());
            _userRepository.Received().Update(user);
            _userRepository.Received().SaveChanges();
        }

        [Fact]
        public async Task UpdateProfile_ValidationFailed_ShouldReturnBadRequest()
        {
            // Arrange 
            var updateProfileDTO = DummyData.ProfileDTOFaker.Generate();
            var user = DummyData.UserFaker.Generate();

            _sut.ControllerContext = FakeControllerContext.GetLoggedInUserContextFor(user);
            _profileValidator.SetupFail();


            // Act 
            var result = await _sut.Update(updateProfileDTO);

            // Assert 
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateProfile_UserNotFound_ShouldReturnBadRequest()
        {
            // Arrange 
            var updateProfileDTO = DummyData.ProfileDTOFaker.Generate();
            var user = DummyData.UserFaker.Generate();
            var identityUser = user.MapToIdentityUser();

            _um.FindByNameAsync(identityUser.Email).Returns(identityUser);
            _profileValidator.SetupPass();
            _sut.ControllerContext = FakeControllerContext.GetLoggedInUserContextFor(user);
            _userRepository.GetBy(user.Email).ReturnsNull();

            // Act 
            var result = await _sut.Update(updateProfileDTO);

            // Assert 
            result.Should().BeOfType<BadRequestResult>();
        }


        [Fact]
        public async Task UpdateProfile_UserNotLoggedIn_FailsAndReturnsBadRequestResult()
        {
            // Arrange 
            var updateProfileDTO = DummyData.ProfileDTOFaker.Generate();
            _sut.ControllerContext = FakeControllerContext.NoLoggedInUserContext;

            // Act 
            var result = await _sut.Update(updateProfileDTO);

            // Assert 
            result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}