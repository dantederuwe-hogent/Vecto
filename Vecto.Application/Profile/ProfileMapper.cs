using Microsoft.AspNetCore.Identity;
using Vecto.Core.Entities;

namespace Vecto.Application.Profile
{
    public static class ProfileMapper
    {
        public static ProfileDTO MapToDTO(this User user) =>
            new ProfileDTO
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

        public static IdentityUser MapToIdentityUser(this User user) =>
            new IdentityUser
            {
                Email = user.Email,
                UserName = user.Email
            };

        public static void UpdateWith(this User user, ProfileDTO model)
        {
            user.Email = model.Email ?? user.Email;
            user.FirstName = model.FirstName ?? model.FirstName;
            user.LastName = model.LastName ?? user.LastName;
        }

        public static void UpdateWith(this IdentityUser identityUser, ProfileDTO model)
        {
            identityUser.Email = model.Email ?? identityUser.Email;
            identityUser.UserName = model.Email ?? identityUser.UserName;
        }
    }
}
