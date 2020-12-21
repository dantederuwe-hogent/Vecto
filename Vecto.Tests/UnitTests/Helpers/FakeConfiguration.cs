using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Vecto.Tests.UnitTests.Helpers
{
    static class FakeConfiguration
    {
        public static IConfiguration Get()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                { "JWT:Secret", "dummyJWTSecretKeyForTestingPurposes" },
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }
    }
}
