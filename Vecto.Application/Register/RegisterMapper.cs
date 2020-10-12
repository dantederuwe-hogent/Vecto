using Microsoft.AspNetCore.Identity;
using Vecto.Core.Entities;

namespace Vecto.Application.Register
{
    public static class RegisterMapper
    {
        public static User MapToUser(this RegisterDTO model) =>
            new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

        public static IdentityUser MapToIdentityUser(this RegisterDTO model) =>
            new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
    }
}
