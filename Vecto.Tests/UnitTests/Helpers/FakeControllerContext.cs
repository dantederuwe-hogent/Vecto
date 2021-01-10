using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vecto.Core.Entities;

namespace Vecto.Tests.UnitTests.Helpers
{
    public static class FakeControllerContext
    {
        public static readonly ControllerContext NoLoggedInUserContext =
            new ControllerContext { HttpContext = new DefaultHttpContext { User = null } };

        public static ControllerContext GetLoggedInUserContextFor(User user)
        {
            var identityUser = new GenericIdentity(user.Email);

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identityUser) }
            };
        }
    }
}
