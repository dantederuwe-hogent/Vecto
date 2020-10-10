using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using System.Collections.Generic;

namespace Vecto.Tests.UnitTests.Api
{
    public static class FakeValidation
    {
        /// <summary>Generic method to setup a passing validator</summary>
        public static void SetupPass<T>(this IValidator<T> validator) =>
            validator.Validate(Arg.Any<T>()).Returns(new ValidationResult());

        /// <summary>Generic method to setup a failing validator</summary>
        public static void SetupFail<T>(this IValidator<T> validator) =>
            validator.Validate(Arg.Any<T>()).Returns(new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("TestProperty", "TestErrorMessage") }));
    }
}
