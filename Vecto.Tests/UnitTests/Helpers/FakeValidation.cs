using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace Vecto.Tests.UnitTests.Helpers
{
    public static class FakeValidation
    {
        /// <summary>Generic method to setup a passing validator</summary>
        public static void SetupPass<T>(this IValidator<T> validator)
        {
            validator.Validate(Arg.Any<T>()).Returns(new ValidationResult());
            validator.ValidateAsync(Arg.Any<T>()).Returns(new ValidationResult());
        }

        /// <summary>Generic method to setup a failing validator</summary>
        public static void SetupFail<T>(this IValidator<T> validator)
        {
            var failures = new List<ValidationFailure>() { new ValidationFailure("TestProperty", "TestErrorMessage") };
            validator.Validate(Arg.Any<T>()).Returns(new ValidationResult(failures));
            validator.ValidateAsync(Arg.Any<T>()).Returns(new ValidationResult(failures));
        }
    }
}
