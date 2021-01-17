using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vecto.Application.Items;
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
        Task<IEnumerable<Section>> GetTripSections(Guid tripId);

        [Post("/trips/{tripId}/sections")]
        Task<IEnumerable<Section>> AddTripSection(Guid tripId, [Body] SectionDTO model);

        [Patch("/trips/{tripId}/sections/{sectionId}")]
        Task<Section> UpdateTripSection(Guid tripid, Guid sectionId, [Body] SectionDTO model);

        [Delete("/trips/{tripId}/sections/{sectionId}")]
        Task DeleteTripSection(Guid tripid, Guid sectionId);

        [Get("/sections/types")]
        Task<IEnumerable<string>> GetSectionTypes();

        [Get("/trips/{tripId}/sections/{sectionId}/items")]
        Task<IEnumerable<TodoItem>> GetTodoItems(Guid tripId, Guid sectionId);

        [Get("/trips/{tripId}/sections/{sectionId}/items")]
        Task<IEnumerable<PackingItem>> GetPackingItems(Guid tripId, Guid sectionId);
        
        [Post("/trips/{tripId}/sections/{sectionId}/items")]
        Task<TodoItem> AddTodoItem(Guid tripId, Guid sectionId, [Body] ItemDTO model);
        
        [Post("/trips/{tripId}/sections/{sectionId}/items")]
        Task<PackingItem> AddPackingItem(Guid tripId, Guid sectionId, [Body] ItemDTO model);

        [Post("/trips/{tripId}/sections/{sectionId}/items/{itemId}/toggle")]
        Task ToggleItem(Guid tripId, Guid sectionId, Guid itemId);

        [Delete("/trips/{tripId}/sections/{sectionId}/items/{itemId}")]
        Task DeleteItem(Guid tripId, Guid sectionId, Guid itemId);

        [Get("/trips/{tripId}/progress")]
        Task<float> GetTripProgress(Guid tripId);
    }
}