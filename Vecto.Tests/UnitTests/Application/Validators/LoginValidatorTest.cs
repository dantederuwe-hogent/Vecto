using FluentValidation.TestHelper;
using Vecto.Application.Login;
using Vecto.Infrastructure.Data;
using Xunit;

namespace Vecto.Tests.UnitTests.Application.Validators
{
    public class LoginValidatorTest
    {
        private readonly LoginValidator _sut;

        public LoginValidatorTest()
        {
            _sut = new LoginValidator();
        }

        [Fact]
        public void Validate_GoodUser_ShouldPass()
        {
            // Arrange
            var loginDTO = DummyData.LoginDTOFaker.Generate();

            // Act
            var result = _sut.TestValidate(loginDTO);

            // Assert          
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("           ")]
        [InlineData("plainaddress")]
        [InlineData("#@%^%#$@#$@#.com")]
        [InlineData("@example.com")]
        [InlineData("Joe Smith <email@example.com>")]
        [InlineData("email.example.com")]
        [InlineData("email@example@example.com")]
        [InlineData(".email@example.com")]
        [InlineData("email.@example.com")]
        [InlineData("あいうえお@example.com")]
        [InlineData("email@example.com (Joe Smith)")]
        [InlineData("email@example")]
        [InlineData("email@-example.com")]
        [InlineData("email@example..com")]
        public void Validate_BadEmail_ShouldFail(string email)
        {
            // Arrange
            var loginDTO = DummyData.LoginDTOFaker.Generate();
            loginDTO.Email = email;

            // Act
            var result = _sut.TestValidate(loginDTO);

            // Assert          
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("           ")]
        [InlineData("2short")]
        public void Validate_BadPassword_ShouldFail(string password)
        {
            // Arrange
            var loginDTO = DummyData.LoginDTOFaker.Generate();
            loginDTO.Password = password;

            // Act
            var result = _sut.TestValidate(loginDTO);

            // Assert          
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}
