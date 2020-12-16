using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vecto.Application.Login;
using Vecto.Application.Profile;
using Vecto.Application.Register;
using Vecto.Application.Trips;

namespace Vecto.UWP.Services
{
    public interface IApiService
    {
        [Post("/login")]
        Task<string> Login([Body] LoginDTO loginDTO);

        [Post("/register")]
        Task<string> Register(RegisterDTO registerDTO);

        [Get("/profile")]
        Task<ProfileDTO> GetProfile();

        [Get("/trips")]
        Task<IEnumerable<TripDTO>> GetTrips();

        [Post("/trips")]
        Task<IEnumerable<TripDTO>> AddTrip(TripDTO tripDTO);
    }
}