using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DamianTourBackend.Tests.UnitTests.Api
{
    static class FakeConfiguration
    {
        public static IConfiguration Get()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"JWT:Secret", "dummyJWTSecretKeyForTestingPurposes"},
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }
    }
}
