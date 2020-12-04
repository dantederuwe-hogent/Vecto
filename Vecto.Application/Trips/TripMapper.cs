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
    }
}