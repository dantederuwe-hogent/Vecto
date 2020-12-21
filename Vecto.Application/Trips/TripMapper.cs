using Vecto.Application.Register;
using Vecto.Core.Entities;

namespace Vecto.Application.Trips
{
    public static class TripMapper
    {
        public static Trip MapToTrip(this TripDTO model) =>
            new Trip
            {
                Name = model.Name,
                StartDateTime = model.StartDateTime,
                EndDateTime = model.EndDateTime
            };

        public static void UpdateWith(this Trip trip, TripDTO model)
        {
            trip.Name = model.Name ?? trip.Name;
            trip.EndDateTime = model.EndDateTime ?? trip.EndDateTime;
            trip.StartDateTime = model.StartDateTime ?? trip.EndDateTime;
        }
    }
}
