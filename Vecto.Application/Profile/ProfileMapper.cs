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
    }
}
