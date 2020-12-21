using FluentAssertions;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace Vecto.Tests.UnitTests.Helpers
{
    public static class AssertionExtensions
    {
        public static void BeJwtToken(this StringAssertions assertions)
        {
            assertions.MatchRegex("^[A-Za-z0-9-_=]+\\.[A-Za-z0-9-_=]+\\.?[A-Za-z0-9-_.+/=]*$");
        }

        public static void BeOkTokenResponse(this ObjectAssertions assertions)
        {
            assertions.BeOfType<OkObjectResult>().Which.Value.ToString().Should().BeJwtToken();
        }

        public static void BeCreatedTokenResponse(this ObjectAssertions assertions)
        {
            assertions.BeOfType<CreatedResult>().Which.Value.ToString().Should().BeJwtToken();
        }

        public static void BeOkObjectEquivalentTo(this ObjectAssertions assertions, object expectation)
        {
            assertions.BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(expectation);
        }
    }
}
