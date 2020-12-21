using System;

namespace Vecto.Core.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
