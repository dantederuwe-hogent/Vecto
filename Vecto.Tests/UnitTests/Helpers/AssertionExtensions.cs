using Bogus.Extensions;
using FluentAssertions;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace Vecto.Tests.UnitTests.Helpers
{
    public static class AssertionExtensions
    {
        public static AndConstraint<StringAssertions> BeJwtToken(this StringAssertions assertions)
        {
            return assertions.MatchRegex("^[A-Za-z0-9-_=]+\\.[A-Za-z0-9-_=]+\\.?[A-Za-z0-9-_.+/=]*$");
        }

        public static AndConstraint<StringAssertions> BeOkTokenResponse(this ObjectAssertions assertions)
        {
            return assertions.BeOfType<OkObjectResult>().Which.Value.ToString().Should().BeJwtToken();
        }
    }
}