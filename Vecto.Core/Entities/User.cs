using System.Collections.Generic;

namespace Vecto.Core.Entities
{
    public class User : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public IList<Trip> Trips { get; } = new List<Trip>();

        public User() { }
    }
}
