using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vecto.Application.Login;
using Vecto.Application.Profile;
using Vecto.Application.Register;
using Vecto.Application.Sections;
using Vecto.Application.Trips;
using Vecto.Core.Entities;

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
        Task<IEnumerable<Trip>> GetTrips();

        [Post("/trips")]
        Task<IEnumerable<TripDTO>> AddTrip(TripDTO tripDTO);

        [Get("/trips/{id}")]
        Task<Trip> GetTrip(Guid id);

        [Patch("/trips/{id}")]
        Task<Trip> UpdateTrip(Guid id, TripDTO tripDTO);

        [Get("/trips/{tripId}/sections")]
        Task<IEnumerable<SectionDTO>> GetTripSections(Guid tripId);

        [Post("/trips/{tripId}/sections")]
        Task<IEnumerable<SectionDTO>> AddTripSection(Guid tripId, [Body] SectionDTO model);

        [Get("/sections/types")]
        Task<IList<string>> GetSectionTypes();
    }
}