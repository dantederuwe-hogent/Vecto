using Microsoft.AspNetCore.Identity;

namespace Vecto.Application.Login
{
    public static class LoginMapper
    {
        public static IdentityUser ToIdentityUser(this LoginDTO model) =>
            new IdentityUser() { UserName = model.Email, Email = model.Email };
    }
}
