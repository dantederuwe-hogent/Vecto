using System;
using System.Collections.Generic;
using Vecto.Core.Interfaces;

namespace Vecto.Core.Entities
{
    public class Trip : EntityBase
    {
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public IList<ISection> Sections { get; } = new List<ISection>();

        public Trip() { }
    }
}