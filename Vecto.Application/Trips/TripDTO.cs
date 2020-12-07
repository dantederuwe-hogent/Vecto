using System;

namespace Vecto.Application.Trips
{
    public class TripDTO
    {
        public string Name { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}