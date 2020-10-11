using FluentValidation.TestHelper;
using Vecto.Application.Register;
using Xunit;
using Xunit.Abstractions;

namespace Vecto.Tests.UnitTests.Application.Validators
{
    public class RegisterValidatorTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly RegisterValidator _sut;

        public RegisterValidatorTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _sut = new RegisterValidator();
        }

        [Fact]
        public void Validate_GoodUser_ShouldPass()
        {
            // Arrange
            var registerDTO = DummyData.RegisterDTOFaker.Generate();

            // Act
            var result = _sut.TestValidate(registerDTO);

            // Assert          
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                                           ")]
        public void Validate_BadFirstName_ShouldFail(string name)
        {
            // Arrange
            var registerDTO = DummyData.RegisterDTOFaker.Generate();
            registerDTO.FirstName = name;

            // Act
            var result = _sut.TestValidate(registerDTO);

            // Assert          
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                                           ")]
        public void Validate_BadLastName_ShouldFail(string name)
        {
            // Arrange
            var registerDTO = DummyData.RegisterDTOFaker.Generate();
            registerDTO.LastName = name;

            // Act
            var result = _sut.TestValidate(registerDTO);

            // Assert          
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Theory]
        [InlineData("TestPassword", "testPassword")]
        [InlineData("TestPassword", "Test Password")]
        [InlineData("Test-Password", "test-password")]
        [InlineData("TestPassword", "testpassword")]
        [InlineData("TestPassword", "Test0Password")]
        [InlineData("TestPassword", "TestPassw0rd")]
        public void Validate_NonMatchingPasswords_ShouldFail(string pass, string confirm)
        {
            // Arrange
            var registerDTO = DummyData.RegisterDTOFaker.Generate();
            registerDTO.Password = pass;
            registerDTO.ConfirmPassword = confirm;

            // Act
            var result = _sut.TestValidate(registerDTO);

            // Assert          
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }
    }
}
